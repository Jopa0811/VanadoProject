-- DROP PROCEDURE public.prc_dodaj_stroj(text);

CREATE OR REPLACE PROCEDURE public.prc_dodaj_stroj(IN p_naziv text)
 LANGUAGE plpgsql
AS $$
DECLARE
BEGIN
    IF EXISTS (
        SELECT 1 FROM Strojevi WHERE LOWER(Naziv) = LOWER(p_naziv)
    ) THEN
        RAISE EXCEPTION 'Stroj s nazivom "%" već postoji.', p_naziv;
    END IF;

    INSERT INTO Strojevi (Naziv)
    VALUES (p_naziv);
END;
$$
;


-- DROP PROCEDURE public.prc_dodaj_kvar(int4, text, timestamptz, timestamptz, text, text);

CREATE OR REPLACE PROCEDURE public.prc_dodaj_kvar(IN p_stroj_id integer, IN p_opis text, IN p_vrijeme_pocetka timestamp with time zone, IN p_vrijeme_zavrsetka timestamp with time zone, IN p_prioritet text, IN p_status text)
 LANGUAGE plpgsql
AS $$
DECLARE
    v_prioritet prioritet_tip;
    v_status status_tip;
BEGIN
	IF NOT EXISTS (
        SELECT 1 FROM Strojevi WHERE Id = p_stroj_id
    ) THEN
        RAISE EXCEPTION 'Stroj s ID % ne postoji.', p_stroj_id;
    END IF;

    v_prioritet := p_prioritet::prioritet_tip;
    v_status := p_status::status_tip;

    IF EXISTS (
        SELECT 1 FROM Kvarovi WHERE StrojId = p_stroj_id AND Status != 'zatvoren'
    ) THEN
        RAISE EXCEPTION 'Već postoji aktivan kvar za ovaj stroj.';
    END IF;

    INSERT INTO Kvarovi (
        StrojId, Opis, VrijemePocetka, VrijemeZavrsetka, Prioritet, Status
    )
    VALUES (
        p_stroj_id, p_opis, p_vrijeme_pocetka, p_vrijeme_zavrsetka, v_prioritet, v_status
    );
END;
$$
;

-- DROP PROCEDURE public.prc_uredi_stroj_pesimistic(int4, text);

CREATE OR REPLACE PROCEDURE public.prc_uredi_stroj_pesimistic(IN p_id integer, IN p_naziv text)
 LANGUAGE plpgsql
AS $$
DECLARE
BEGIN
	PERFORM 1 FROM Strojevi
    WHERE Id = p_id
    FOR UPDATE;

    IF EXISTS (
        SELECT 1
        FROM Strojevi
        WHERE LOWER(Naziv) = LOWER(p_naziv)
          AND Id <> p_id
    ) THEN
        RAISE EXCEPTION 'Stroj s nazivom "%" već postoji.', p_naziv;
    END IF;

    UPDATE Strojevi
    SET Naziv = p_naziv
    WHERE Id = p_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Stroj s ID-em % nije pronaden.', p_id;
    END IF;
END;
$$
;


-- DROP PROCEDURE public.prc_uredi_kvar_pesimisticki(int4, int4, text, timestamp, timestamp, text, text);

CREATE OR REPLACE PROCEDURE public.prc_uredi_kvar_pesimisticki(IN p_id integer, IN p_stroj_id integer, IN p_opis text, IN p_vrijeme_pocetka timestamp without time zone, IN p_vrijeme_zavrsetka timestamp without time zone, IN p_prioritet text, IN p_status text)
 LANGUAGE plpgsql
AS $$
DECLARE
    v_prioritet prioritet_tip;
    v_status status_tip;
BEGIN
    PERFORM 1 FROM Kvarovi
    WHERE Id = p_id
    FOR UPDATE;

	v_prioritet := p_prioritet::prioritet_tip;
    v_status := p_status::status_tip;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Kvar s ID % nije pronađen.', p_id;
    END IF;

    UPDATE Kvarovi
    SET
        StrojId = p_stroj_id,
        Opis = p_opis,
        VrijemePocetka = p_vrijeme_pocetka,
        VrijemeZavrsetka = p_vrijeme_zavrsetka,
        Prioritet = v_prioritet,
        Status = v_status
    WHERE Id = p_id;
END;
$$
;


-- DROP PROCEDURE public.prc_izbrisi_stroj_pesimisticki(int4);

CREATE OR REPLACE PROCEDURE public.prc_izbrisi_stroj_pesimisticki(IN p_id integer)
 LANGUAGE plpgsql
AS $$
BEGIN
    PERFORM 1 FROM Strojevi
    WHERE Id = p_id
    FOR UPDATE;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Stroj s ID % ne postoji.', p_id;
    END IF;

    DELETE FROM Strojevi WHERE Id = p_id;
END;
$$
;
