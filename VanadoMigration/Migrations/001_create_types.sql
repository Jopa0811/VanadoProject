-- Type: prioritet_tip

-- DROP TYPE IF EXISTS public.prioritet_tip;

CREATE TYPE public.prioritet_tip AS ENUM
    ('nizak', 'srednji', 'visok');

ALTER TYPE public.prioritet_tip
    OWNER TO postgres;


-- Type: status_tip

-- DROP TYPE IF EXISTS public.status_tip;

CREATE TYPE public.status_tip AS ENUM
    ('otvoren', 'zatvoren');

ALTER TYPE public.status_tip
    OWNER TO postgres;
