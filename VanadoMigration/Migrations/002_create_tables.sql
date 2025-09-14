CREATE TABLE public.strojevi (
	id serial4 NOT NULL,
	naziv varchar(255) NOT NULL,
	CONSTRAINT strojevi_naziv_key UNIQUE (naziv),
	CONSTRAINT strojevi_pkey PRIMARY KEY (id)
);

-- public.kvarovi definition

-- Drop table

-- DROP TABLE public.kvarovi;

CREATE TABLE public.kvarovi (
	id serial4 NOT NULL,
	strojid int4 NOT NULL,
	opis text NOT NULL,
	vrijemepocetka timestamp NOT NULL,
	vrijemezavrsetka timestamp NULL,
	prioritet public."prioritet_tip" NOT NULL,
	status public."status_tip" NOT NULL,
	CONSTRAINT kvarovi_pkey PRIMARY KEY (id)
);


-- public.kvarovi foreign keys

ALTER TABLE public.kvarovi ADD CONSTRAINT kvarovi_strojid_fkey FOREIGN KEY (strojid) REFERENCES public.strojevi(id);