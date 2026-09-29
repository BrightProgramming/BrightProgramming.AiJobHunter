CREATE TABLE IF NOT EXISTS jobs
(
    id uuid PRIMARY KEY,
    title varchar(200) NOT NULL,
    company varchar(200) NOT NULL,
    url varchar(2048) NOT NULL
);
