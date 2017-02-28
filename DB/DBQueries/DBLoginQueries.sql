select * from authentication;

select * from student_info;

select * from professor_info;

select * from authentication as a
JOIN student_info as s where a.email = s.email;

select * from authentication as a
JOIN professor_info as p where a.email = p.email;

delete from authentication where email="vmiller@ramapo.edu";