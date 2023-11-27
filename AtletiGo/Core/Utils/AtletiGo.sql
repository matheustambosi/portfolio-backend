CREATE TABLE IF NOT EXISTS public.atleta
(
    codigo uuid NOT NULL,
    codigousuario uuid NOT NULL,
    codigomodalidade uuid NOT NULL,
    CONSTRAINT atleta_pkey PRIMARY KEY (codigo)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.atleta
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public.atletica
(
    codigo uuid NOT NULL,
    nome character varying(100) COLLATE pg_catalog."default" NOT NULL,
    dtcriacao date NOT NULL,
    dtalteracao date NOT NULL,
    situacao smallint NOT NULL,
    universidade character varying COLLATE pg_catalog."default" NOT NULL,
    cidade character varying COLLATE pg_catalog."default" NOT NULL,
    estado character varying COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT atletica_pkey PRIMARY KEY (codigo)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.atletica
    OWNER to postgres;
	
CREATE TABLE IF NOT EXISTS public.evento
(
    codigo uuid NOT NULL,
    codigoatletica uuid NOT NULL,
    dtevento date NOT NULL,
    nomeevento character varying(200) COLLATE pg_catalog."default" NOT NULL,
    enderecoevento character varying(200) COLLATE pg_catalog."default" NOT NULL,
    visivelsematletica boolean NOT NULL,
    visivelcomatletica boolean NOT NULL,
    visivelatleta boolean NOT NULL,
    codigomodalidade uuid,
    dtcriacao date NOT NULL,
    dtalteracao date NOT NULL,
    situacao smallint NOT NULL,
    CONSTRAINT evento_pkey PRIMARY KEY (codigo)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.evento
    OWNER to postgres;

CREATE TABLE IF NOT EXISTS public.modalidade
(
    codigo uuid NOT NULL,
    descricao character varying(100) COLLATE pg_catalog."default" NOT NULL,
    buscandoatletas boolean NOT NULL,
    situacao smallint NOT NULL,
    dtcriacao date NOT NULL,
    dtalteracao date NOT NULL,
    codigoatletica uuid NOT NULL,
    CONSTRAINT modalidade_pkey PRIMARY KEY (codigo)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.modalidade
    OWNER to postgres;
	
CREATE TABLE IF NOT EXISTS public.qrcode
(
    codigo uuid NOT NULL,
    codigoatletica uuid NOT NULL,
    situacao smallint NOT NULL,
    duracaodias integer NOT NULL,
    dtcriacao date NOT NULL,
    descricao character varying(200) COLLATE pg_catalog."default"
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.qrcode
    OWNER to postgres;
	
CREATE TABLE IF NOT EXISTS public.usuario
(
    codigo uuid NOT NULL,
    nome character varying(200) COLLATE pg_catalog."default" NOT NULL,
    hashsenha character varying(300) COLLATE pg_catalog."default" NOT NULL,
    email character varying(200) COLLATE pg_catalog."default" NOT NULL,
    tipousuario smallint NOT NULL,
    dtcriacao date NOT NULL,
    dtalteracao date NOT NULL,
    codigoatletica uuid,
    CONSTRAINT usuario_pkey PRIMARY KEY (codigo)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.usuario
    OWNER to postgres;