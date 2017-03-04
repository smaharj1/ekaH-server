SELECT * FROM courses;

insert into courses(courseID, year, semester, professorID, days, startTime, endTime, courseName, courseDescription) 
values('crs-test', 2016, 'f', 'vmiller@ramapo.edu','MR','2:00:00', '4:00:00', '','');

truncate table courses;

update courses set year = 2017, semester='F', days='TF', startTime='', endTime='', courseName='', courseDescription ='';