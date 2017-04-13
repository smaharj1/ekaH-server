SELECT * FROM ekah.assignment;

insert into assignment(courseID, projectNum, projectTitle, weight, deadline, content, attachments) values
('CRS-F2017vmiller120TF', 2, "Stack Practice", 0, '2017-04-19 23:59:59', "Implement the stack from the following", "")
ON DUPLICATE KEY UPDATE courseID = 'CRS-F2017vmiller120TF', projectNum = 1, projectTitle = "Global Warming", weight = 20, deadline = '2017-04-05 23:59:59';

(@cid, @projectNum, @title, @weight, @deadline, @content, @attachments);