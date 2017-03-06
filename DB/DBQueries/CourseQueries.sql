SELECT * FROM courses;

insert into courses(courseID, year, semester, professorID, days, startTime, endTime, courseName, courseDescription) 
values('CRS-F2017amruth20TF', 2017, 'F', 'amruth@ramapo.edu','TF','2:00:00', '4:00:00', 'OPL','');

truncate table courses;

update courses set year = 2017, semester='F', days='TF', startTime='', endTime='', courseName='', courseDescription ='';

delete from courses where courseID='CRS-F2017amruth20TF' and professorID='amruth@ramapo.edu';