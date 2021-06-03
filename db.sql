# create schema flashmusic_db collate latin1_swedish_ci;

create table user
(
	userid int auto_increment
		primary key not null,
	username varchar(255) not null,
	password varchar(255) not null,
	email varchar(255),
	avatar varchar(255)
)
charset=utf8;

create table product
(
	productid varchar(36)
		primary key not null,
	categoryid int,
	price double(16, 2) not null,
	name varchar(255) not null,
	picurl varchar(255)
)
charset=utf8;

create table cart
(
	userid int not null,
	productid varchar(36) not null,
	num int,
	state int,
	primary key (userid, productid)
)
charset=utf8;

create table history
(
	userid int not null,
	productid varchar(36) not null,
	paytime datetime not null,
	num int,
	primary key (userid, productid, paytime)
)
charset=utf8;