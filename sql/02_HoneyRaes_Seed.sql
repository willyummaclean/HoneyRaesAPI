INSERT INTO Customer (Id, Name, Address)
VALUES
    (1, 'Glidden Masters', '2802 Zula Locks Dr'),
    (2, 'Filipe Gonzaga', '56849 Fadel Gateway'),
    (3, 'Roger Talos', '7346 Ritchie Road'),
    (4, 'Zelda Reigns Cobbler', '123 Fake St'),
    (5, 'Juan Carlos Wong', '888 Panama Rd'),
    (6, 'Griper Bunson', '1244 Carlitos Way');

INSERT INTO Employee (Id, Name, Specialty)
VALUES
    (1, 'Willy Bender', 'Macs & PCs'),
    (2, 'Cordon Blue', 'Viruses & Malware'),
    (3, 'Bunson HoneyDew', 'Science'),
    (4, 'Beeker', 'Beeping'),
    (5, 'Rick', 'Drinking');

INSERT INTO ServiceTicket (Id, CustomerId, EmployeeId, Description, Emergency, DateCompleted)
VALUES
    (1, 3, 2, 'computer on fire', false, '2023-07-31'),
    (2, 3, 3, 'ipad ate my homework', false, '2023-08-16'),
    (3, 1, 2, 'This is a ticket', true, NULL),
    (4, 1, 2, 'android wants to eliminate humanity', false, NULL),
    (5, 1, 1, 'help', true, '2024-08-05'),
    (6, 4, 3, 'Gerbil in Printer', true, '2024-08-05'),
    (7, 5, 4, 'Ipod stuck on nickelback', true, '2024-08-01'),
    (8, 1, NULL, 'hair dryer wont connect to wifi', false, NULL);