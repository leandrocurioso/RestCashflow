SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
--  Table structure for `day_balance`
-- ----------------------------
DROP TABLE IF EXISTS `day_balance`;
CREATE TABLE `day_balance` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `date` date NOT NULL,
  `total_entry` decimal(11,4) NOT NULL,
  `total_out` decimal(11,4) NOT NULL,
  `interest` decimal(11,4) NOT NULL,
  PRIMARY KEY (`id`,`date`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- ----------------------------
--  Table structure for `financial_entry`
-- ----------------------------
DROP TABLE IF EXISTS `financial_entry`;
CREATE TABLE `financial_entry` (
  `id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `entry_type` int(11) NOT NULL,
  `description` varchar(1000) NOT NULL,
  `destination_account` varchar(50) NOT NULL,
  `destination_bank` int(11) NOT NULL,
  `destination_cpf_cnpj` varchar(20) NOT NULL,
  `account_type` int(11) NOT NULL,
  `value` decimal(11,4) NOT NULL,
  `charge` decimal(11,4) NOT NULL,
  `entry_date` datetime NOT NULL,
  `created_at` datetime NOT NULL,
  `conciled` int(11) NOT NULL DEFAULT 0,
  `conciled_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

SET FOREIGN_KEY_CHECKS = 1;
