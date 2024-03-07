CREATE DATABASE  IF NOT EXISTS `minecoloniesdb` /*!40100 DEFAULT CHARACTER SET utf8mb3 */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `minecoloniesdb`;
-- MySQL dump 10.13  Distrib 8.0.36, for Win64 (x86_64)
--
-- Host: localhost    Database: minecoloniesdb
-- ------------------------------------------------------
-- Server version	8.0.33

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `colonies`
--

DROP TABLE IF EXISTS `colonies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `colonies` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `Worlds_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Colonies_Worlds1_idx` (`Worlds_id`),
  CONSTRAINT `fk_Colonies_Worlds1` FOREIGN KEY (`Worlds_id`) REFERENCES `worlds` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `colonies`
--

LOCK TABLES `colonies` WRITE;
/*!40000 ALTER TABLE `colonies` DISABLE KEYS */;
/*!40000 ALTER TABLE `colonies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `items_in_storage`
--

DROP TABLE IF EXISTS `items_in_storage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `items_in_storage` (
  `id` int NOT NULL AUTO_INCREMENT,
  `display_name` varchar(45) NOT NULL,
  `mincraft_name` varchar(45) NOT NULL,
  `amount` int NOT NULL,
  `stack_size` int NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `items_in_storage`
--

LOCK TABLES `items_in_storage` WRITE;
/*!40000 ALTER TABLE `items_in_storage` DISABLE KEYS */;
/*!40000 ALTER TABLE `items_in_storage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `items_in_storage_has_recipies`
--

DROP TABLE IF EXISTS `items_in_storage_has_recipies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `items_in_storage_has_recipies` (
  `Items_in_Storage_id` int NOT NULL,
  `Recipies_id` int NOT NULL,
  PRIMARY KEY (`Items_in_Storage_id`,`Recipies_id`),
  KEY `fk_Items_in_Storage_has_Recipies_Recipies1_idx` (`Recipies_id`),
  KEY `fk_Items_in_Storage_has_Recipies_Items_in_Storage1_idx` (`Items_in_Storage_id`),
  CONSTRAINT `fk_Items_in_Storage_has_Recipies_Items_in_Storage1` FOREIGN KEY (`Items_in_Storage_id`) REFERENCES `items_in_storage` (`id`),
  CONSTRAINT `fk_Items_in_Storage_has_Recipies_Recipies1` FOREIGN KEY (`Recipies_id`) REFERENCES `recipies` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `items_in_storage_has_recipies`
--

LOCK TABLES `items_in_storage_has_recipies` WRITE;
/*!40000 ALTER TABLE `items_in_storage_has_recipies` DISABLE KEYS */;
/*!40000 ALTER TABLE `items_in_storage_has_recipies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipies`
--

DROP TABLE IF EXISTS `recipies`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipies` (
  `id` int NOT NULL,
  `patrern` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipies`
--

LOCK TABLES `recipies` WRITE;
/*!40000 ALTER TABLE `recipies` DISABLE KEYS */;
/*!40000 ALTER TABLE `recipies` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recipies_has_pattern`
--

DROP TABLE IF EXISTS `recipies_has_pattern`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recipies_has_pattern` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Recipies_id` int NOT NULL,
  `Colonies_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Recipies_Has_Pattern_Recipies1_idx` (`Recipies_id`),
  KEY `fk_Recipies_Has_Pattern_Colonies1_idx` (`Colonies_id`),
  CONSTRAINT `fk_Recipies_Has_Pattern_Colonies1` FOREIGN KEY (`Colonies_id`) REFERENCES `colonies` (`id`),
  CONSTRAINT `fk_Recipies_Has_Pattern_Recipies1` FOREIGN KEY (`Recipies_id`) REFERENCES `recipies` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recipies_has_pattern`
--

LOCK TABLES `recipies_has_pattern` WRITE;
/*!40000 ALTER TABLE `recipies_has_pattern` DISABLE KEYS */;
/*!40000 ALTER TABLE `recipies_has_pattern` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requests`
--

DROP TABLE IF EXISTS `requests`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `requests` (
  `id` int NOT NULL AUTO_INCREMENT,
  `x` int NOT NULL,
  `y` int NOT NULL,
  `z` int NOT NULL,
  `name` varchar(45) NOT NULL,
  `type` varchar(45) NOT NULL,
  `description` varchar(45) NOT NULL,
  `completed` tinyint NOT NULL,
  `Colonies_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Requests_Colonies1_idx` (`Colonies_id`),
  CONSTRAINT `fk_Requests_Colonies1` FOREIGN KEY (`Colonies_id`) REFERENCES `colonies` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requests`
--

LOCK TABLES `requests` WRITE;
/*!40000 ALTER TABLE `requests` DISABLE KEYS */;
/*!40000 ALTER TABLE `requests` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `requests_has_items_in_storage`
--

DROP TABLE IF EXISTS `requests_has_items_in_storage`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `requests_has_items_in_storage` (
  `Requests_id` int NOT NULL,
  `Items_in_Storage_id` int NOT NULL,
  `aumount` int NOT NULL,
  `deliverd` tinyint NOT NULL,
  PRIMARY KEY (`Requests_id`,`Items_in_Storage_id`),
  KEY `fk_Requests_has_Items_in_Storage_Items_in_Storage1_idx` (`Items_in_Storage_id`),
  KEY `fk_Requests_has_Items_in_Storage_Requests1_idx` (`Requests_id`),
  CONSTRAINT `fk_Requests_has_Items_in_Storage_Items_in_Storage1` FOREIGN KEY (`Items_in_Storage_id`) REFERENCES `items_in_storage` (`id`),
  CONSTRAINT `fk_Requests_has_Items_in_Storage_Requests1` FOREIGN KEY (`Requests_id`) REFERENCES `requests` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `requests_has_items_in_storage`
--

LOCK TABLES `requests_has_items_in_storage` WRITE;
/*!40000 ALTER TABLE `requests_has_items_in_storage` DISABLE KEYS */;
/*!40000 ALTER TABLE `requests_has_items_in_storage` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `worlds`
--

DROP TABLE IF EXISTS `worlds`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `worlds` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(45) NOT NULL,
  `Users_id` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_Worlds_Users_idx` (`Users_id`),
  CONSTRAINT `fk_Worlds_Users` FOREIGN KEY (`Users_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `worlds`
--

LOCK TABLES `worlds` WRITE;
/*!40000 ALTER TABLE `worlds` DISABLE KEYS */;
/*!40000 ALTER TABLE `worlds` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-03-07 15:16:56
