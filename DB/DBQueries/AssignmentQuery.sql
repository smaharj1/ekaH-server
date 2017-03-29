SELECT * FROM ekah.assignment;

insert into assignment(courseID, projectNum, projectTitle, weight, deadline, content, attachments) values
('CRS-S2017mserban140MR', 1, "Global Warning", 0, '2017-04-05 23:59:59', "What do poor countries fear the most for global warning?", "")
ON DUPLICATE KEY UPDATE courseID = 'CRS-S2017mserban140MR', projectNum = 1, projectTitle = "Global Warming", weight = 20, deadline = '2017-04-05 23:59:59';

(@cid, @projectNum, @title, @weight, @deadline, @content, @attachments);