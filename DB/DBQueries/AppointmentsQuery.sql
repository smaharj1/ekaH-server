# This is for appointments
select * from appointments;

insert into appointments(scheduleID, startTime, endTime)
select id, '16:30:00', '17:30:00' from officehours where startDtime = '2017-03-23 16:00:00';

update appointments set attendeeID='smaharj1@ramapo.edu' where id=1;