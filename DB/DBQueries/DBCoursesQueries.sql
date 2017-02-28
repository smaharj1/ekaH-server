select * from courses;

select * from courses 
where courseID = "";

insert into courses(courseNum, courseName, year, professorID, semester)
values("COB552", "CSII", 2017, "vmiller@ramapo.edu", "fall");

truncate table courses;

alter table courses auto_increment=1000;