select * from professor_info;

insert into professor_info(firstName, lastName, email, department, education, university, concentration, streetAdd1, streetAdd2, state, zip)
values ("Scott", "Frees", "sfrees@ramapo.edu", "CS", "PhD", "Lehigh University", "JavaScript", "505 Ramapo", "", "NJ", "07430");

update professor_info set firstName='Victor', lastName='Miller', department='CS', education='PhD', university='', concentration='', streetAdd1='', streetAdd2='', state='', zip=''
where email='vmiller@ramapo.edu';