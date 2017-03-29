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
-- Table structure for table `professor_info`
--

DROP TABLE IF EXISTS `professor_info`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `professor_info` (
  `firstName` varchar(15) NOT NULL,
  `lastName` varchar(25) NOT NULL,
  `email` varchar(45) NOT NULL,
  `department` varchar(45) DEFAULT NULL,
  `education` varchar(45) DEFAULT NULL,
  `university` varchar(45) DEFAULT NULL,
  `concentration` varchar(45) DEFAULT NULL,
  `streetAdd1` varchar(45) DEFAULT NULL,
  `streetAdd2` varchar(45) DEFAULT NULL,
  `city` varchar(40) DEFAULT NULL,
  `state` varchar(2) DEFAULT NULL,
  `zip` varchar(10) DEFAULT NULL,
  `phone` varchar(15) DEFAULT NULL,
  PRIMARY KEY (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='This holds the general info of the professors';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `professor_info`
--

LOCK TABLES `professor_info` WRITE;
/*!40000 ALTER TABLE `professor_info` DISABLE KEYS */;
INSERT INTO `professor_info` VALUES ('Amruth','Kumar','amruth@ramapo.edu','Computer Science','','NIT','','505 Ramapo Valley Road','','Mahwah','NJ','','2016847666'),('Mia','Serban','mserban@ramapo.edu','Law','PhD','NYU','Law and Society','Columbia Ave','','Rich City','NY','07546','2016754068'),('Scott','Frees','sfrees@ramapo.edu','CS','PhD','Lehigh University','JavaScript','505 Ramapo','',NULL,'NJ','07430',NULL),('Victor','Miller','vmiller@ramapo.edu','Computer Science','PhD','Stevens','Finance trading','505 Ramapo Valley','','Mahwah','NJ','07512','2016667785');
/*!40000 ALTER TABLE `professor_info` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-03-28 20:09:08
