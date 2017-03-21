select * from student_info;

select * from professor_info;

select * from member_type;

insert into member_type values ("vpandey@ramapo.edu", 1);

select * from studentcourse;

select * from courses;

select * from courses where professorID='vmiller@ramapo.edu';

select * from courses where courseID in (select courseID from studentcourse where studentID='csilber@ramapo.edu');

select * from student_info where email in (select studentID from studentcourse where courseID='CRS-S2017vmiller100MR');

select * from studentcourse where courseID='CRS-S2017vmiller100MR';

alter table studentcourse add foreign key (courseID) references courses(courseID) 
on delete cascade
on update cascade;

insert into studentcourse(courseID, studentID) values ("CRS-F2020amruth160TF", "s@ramapo.edu");

delete from courses where courseID='CRS-F2020amruth160TF';

alter table appointments add foreign key (attendeeID) references student_info(email) 
on delete set null
on update cascade;



insert into officehours(