# This is for appointments
select * from appointments;

insert into appointments(scheduleID, startTime, endTime)
select id, '16:30:00', '17:30:00' from officehours where startDtime = '2017-03-27 16:00:00';

insert into appointments(scheduleID, startTime, endTime)
values (2, '17:00:00', '17:30:00');

update appointments set attendeeID='smaharj1@ramapo.edu' where id=1;

delete from appointments where id =3;

insert into appointments(scheduleID, startTime, endTime, attendeeID) values (
7, '','','');

select * from officehours where startDTime =

select * from appointments where scheduleID=2 and startTime='17:00:00' and endTime='17:30:00';

select * from appointments where scheduleID in (select id from officehours where professorID='amruth@ramapo.edu');