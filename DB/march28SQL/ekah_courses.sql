-- MySQL dump 10.13  Distrib 5.7.12, for Win64 (x86_64)
--
-- Host: localhost    Database: ekah
-- ------------------------------------------------------
-- Server version	5.7.17-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `courses`
--

DROP TABLE IF EXISTS `courses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `courses` (
  `courseID` varchar(60) NOT NULL,
  `year` year(4) NOT NULL,
  `semester` char(1) NOT NULL,
  `professorID` varchar(45) NOT NULL,
  `days` varchar(8) NOT NULL COMMENT 'this should strictly follow SMTWRFA for days of the week.',
  `startTime` time(6) NOT NULL,
  `endTime` time(6) NOT NULL,
  `courseName` varchar(45) DEFAULT NULL,
  `courseDescription` mediumtext,
  PRIMARY KEY (`courseID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `courses`
--

LOCK TABLES `courses` WRITE;
/*!40000 ALTER TABLE `courses` DISABLE KEYS */;
INSERT INTO `courses` VALUES ('CRS-F2016amruth180MR',2016,'F','amruth@ramapo.edu','MR','18:00:00.000000','21:00:00.000000','Computer Science I','This is a beginner course'),('CRS-F2017amruth140MR',2017,'F','amruth@ramapo.edu','MR','14:00:00.000000','16:00:00.000000','OPL',''),('CRS-F2017vmiller120TF',2017,'F','vmiller@ramapo.edu','TF','12:00:00.000000','02:00:00.000000','Financial Modeling','Teach students about how financial models are built'),('CRS-S2017mserban140MR',2017,'S','mserban@ramapo.edu','MR','14:00:00.000000','15:40:00.000000','Ethics','This is a class about how the ethics plays the role'),('CRS-S2017vmiller100MR',2017,'S','vmiller@ramapo.edu','MR','10:00:00.000000','12:00:00.000000','Data Structures','This course teaches about the basics of data structures'),('CRS-S2018amruth180TF',2018,'S','amruth@ramapo.edu','TF','18:00:00.000000','21:00:00.000000','Artificial Intelligence','This is highly advanced course of AI');
/*!40000 ALTER TABLE `courses` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-03-28 20:09:09
