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
-- Table structure for table `officehours`
--

DROP TABLE IF EXISTS `officehours`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `officehours` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `professorID` varchar(45) NOT NULL,
  `startDTime` datetime NOT NULL,
  `endDTime` datetime NOT NULL,
  PRIMARY KEY (`professorID`,`startDTime`,`endDTime`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  CONSTRAINT `officehours_ibfk_1` FOREIGN KEY (`professorID`) REFERENCES `professor_info` (`email`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=45 DEFAULT CHARSET=utf8 COMMENT='This is a generic table saying when the office hours are';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `officehours`
--

LOCK TABLES `officehours` WRITE;
/*!40000 ALTER TABLE `officehours` DISABLE KEYS */;
INSERT INTO `officehours` VALUES (7,'amruth@ramapo.edu','2017-03-27 16:00:00','2017-03-27 18:00:00'),(8,'amruth@ramapo.edu','2017-03-30 16:00:00','2017-03-30 18:00:00'),(9,'amruth@ramapo.edu','2017-04-03 16:00:00','2017-04-03 18:00:00'),(11,'amruth@ramapo.edu','2017-04-10 16:00:00','2017-04-10 18:00:00'),(12,'amruth@ramapo.edu','2017-04-13 16:00:00','2017-04-13 18:00:00'),(13,'amruth@ramapo.edu','2017-04-17 16:00:00','2017-04-17 18:00:00'),(14,'amruth@ramapo.edu','2017-04-20 16:00:00','2017-04-20 18:00:00'),(15,'amruth@ramapo.edu','2017-03-23 16:00:00','2017-03-23 18:00:00'),(21,'amruth@ramapo.edu','2017-04-06 16:00:00','2017-04-06 18:00:00'),(34,'vmiller@ramapo.edu','2017-03-29 17:00:00','2017-03-29 18:00:00'),(35,'vmiller@ramapo.edu','2017-04-05 17:00:00','2017-04-05 18:00:00'),(36,'vmiller@ramapo.edu','2017-04-12 17:00:00','2017-04-12 18:00:00'),(37,'vmiller@ramapo.edu','2017-04-19 17:00:00','2017-04-19 18:00:00'),(38,'vmiller@ramapo.edu','2017-04-26 17:00:00','2017-04-26 18:00:00'),(39,'vmiller@ramapo.edu','2017-05-03 17:00:00','2017-05-03 18:00:00'),(40,'mserban@ramapo.edu','2017-04-03 13:00:00','2017-04-03 14:00:00'),(41,'mserban@ramapo.edu','2017-04-10 13:00:00','2017-04-10 14:00:00'),(42,'mserban@ramapo.edu','2017-04-17 13:00:00','2017-04-17 14:00:00'),(43,'mserban@ramapo.edu','2017-04-24 13:00:00','2017-04-24 14:00:00'),(44,'mserban@ramapo.edu','2017-05-01 13:00:00','2017-05-01 14:00:00');
/*!40000 ALTER TABLE `officehours` ENABLE KEYS */;
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
