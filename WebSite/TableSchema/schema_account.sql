-- Table: public.account

--DROP TABLE public.account;

CREATE TABLE public.account
(
	id serial PRIMARY KEY NOT NULL,
	name varchar(100),
	type varchar(100),
	level decimal,
	parent varchar(100),
	remark varchar(100),
	create_user varchar(100),
	create_time timestamp,
	update_user varchar(100),
	update_time timestamp,
	enable boolean
)

WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.account
    OWNER to postgres;
COMMENT ON TABLE public.account
    IS '科目定義表';