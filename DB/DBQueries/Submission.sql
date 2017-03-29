SELECT * FROM ekah.submission;

set @ssDate = '2017-04-10 12:12:12';
insert into submission(assignmentID, studentID, grade,  submissionDateTime) values
(1, 'smaharj1@ramapo.edu', -1, @ssDate) if @ssDate <=(select id from assignment where assignmentID = id);

insert into submission(assignmentID, studentID, grade, submissionContent, submissionDateTime) values
(@aID, @studentID, @grade, @content, @submissionDateTime)