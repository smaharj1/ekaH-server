select * from officehours;

insert into officehours(professorID, startDtime, endDTime) values ('sfrees@ramapo.edu', '4/20/2017 4:00:00', '4/20/2017 4:30:00')
on duplicate key update professorID='sfrees@ramapo.edu', startDTime='4/20/2017 4:00:00 PM', endDTime='4/20/2017 4:30:00 PM';

select id from officehours where professorID='amruth@ramapo.edu';

select id from officehours where startDtime = '2017-03-23 16:00:00';

delete from officehours where professorID='vmiller@ramapo.edu';

delete from officehours where 

select * from officehours where startDTime >= '2017-03-20 0:00:00' and startDTime <='2017-03-28 0:00:00' and professorID='amruth@ramapo.edu';

select * from professor_info JOIN officehours where professor_info.email = officehours.professorID and officehours.id = (select scheduleID from appointments where id=1);

select * from professor_info where professor_info.email = (select professorID from officehours where officehours.id = (select scheduleID from appointments where id=1));
