select * from officehours;

insert into officehours(professorID, startDtime, endDTime) values ('sfrees@ramapo.edu', '4/20/2017 4:00:00 PM', '4/20/2017 4:30:00 PM')
on duplicate key update professorID='sfrees@ramapo.edu', startDTime='4/20/2017 4:00:00 PM', endDTime='4/20/2017 4:30:00 PM';

select id from officehours where professorID='amruth@ramapo.edu';

select id from officehours where startDtime = '2017-03-23 16:00:00';

delete from officehours where professorID='amruth@ramapo.edu';

delete from officehours where id=1;