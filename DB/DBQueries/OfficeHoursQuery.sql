select * from officehours;

insert into officehours(professorID, startDtime, endDTime) values ('amruth@ramapo.edu', '2017-03-23 16:00:00', '2017-03-23 18:00:00');

select id from officehours where professorID='amruth@ramapo.edu';

select id from officehours where startDtime = '2017-03-23 16:00:00';