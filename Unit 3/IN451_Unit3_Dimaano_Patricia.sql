create role SalesRole;
create role HRRole;
create role CEORole;
go

grant select on Orders to SalesRole;
grant select on Customers to SalesRole;
grant select on Employees to HRRole;
grant select on Orders to CEORole;
grant select on Customers to CEORole;
grant select on Employees to CEORole;
go

create login User_CEO with password = 'ceo';
create user User_CEO for login User_CEO;
alter role CEORole add member User_CEO;
go

create login User_HR with password = 'hr';
create user User_HR for login User_HR;
alter role HRRole add member User_HR;
go

create login User_Sales with password = 'sales';
create user User_Sales for login User_Sales;
alter role SalesRole add member User_Sales;
go