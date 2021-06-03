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
	picurl varchar(255),
	orderid varchar(36)
)
charset=utf8;

create table orders
(
	orderid varchar(36)
		primary key not null,
	num int,
	state int,
	userid int,
	historyid varchar(36)
)
charset=utf8;

create table history
(
	historyid varchar(36)
		primary key not null,
	userid int,
	orderid varchar(36)
)
charset=utf8;

Alter Table product Add constraint foreign key (orderid) references orders(orderid);

Alter Table history Add constraint foreign key (userid) references user(userid);

Alter Table history Add constraint foreign key (orderid) references orders(orderid);

Alter Table orders Add constraint foreign key (userid) references user(userid);

Alter Table orders Add constraint foreign key (historyid) references history(historyid);