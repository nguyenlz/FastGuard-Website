-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Máy chủ: localhost:3307:3307
-- Thời gian đã tạo: Th12 14, 2023 lúc 09:43 AM
-- Phiên bản máy phục vụ: 10.4.28-MariaDB
-- Phiên bản PHP: 8.2.4
create database test_fastguard;

use test_fastguard;
SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Cơ sở dữ liệu: `fastguard`
--

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aspnetroleclaims`
--

CREATE TABLE `aspnetroleclaims` (
  `Id` int(11) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aspnetroles`
--

CREATE TABLE `aspnetroles` (
  `Id` varchar(255) NOT NULL,
  `Name` varchar(256) DEFAULT NULL,
  `NormalizedName` varchar(256) DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `aspnetroles`
--

INSERT INTO `aspnetroles` (`Id`, `Name`, `NormalizedName`, `ConcurrencyStamp`) VALUES
('415a63e9-3b20-4967-9a46-f6fa6e35072c', 'Admin', 'ADMIN', '92e85189-8708-474d-a787-4760adfb97d7'),
('639ecdd4-9670-42b0-a48b-b2d30e630ec4', 'Driver', 'DRIVER', '360de143-2d73-46ea-9b5e-a2dcf451952a'),
('78cb9c57-8e9b-494d-a331-7b123d6f9ee5', 'Employee', 'EMPLOYEE', '46460626-bfdd-4335-a1c2-677850c93b66'),
('c3ec2484-ee4f-4acc-8344-c99f25dc1c3a', 'Customer', 'CUSTOMER', '739b161b-9b2b-4f61-ad68-013407277a02');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aspnetuserclaims`
--

CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL,
  `UserId` varchar(255) NOT NULL,
  `ClaimType` longtext DEFAULT NULL,
  `ClaimValue` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aspnetuserlogins`
--

CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `ProviderDisplayName` longtext DEFAULT NULL,
  `UserId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aspnetuserroles`
--

CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `aspnetuserroles`
--

INSERT INTO `aspnetuserroles` (`UserId`, `RoleId`) VALUES
('27a9cf8c-6f49-40e8-9b2a-7314b436105a', 'c3ec2484-ee4f-4acc-8344-c99f25dc1c3a'),
('4626bb53-48f9-47c5-9732-0fcf918a423a', '639ecdd4-9670-42b0-a48b-b2d30e630ec4'),
('59c09a5c-2b80-4e34-ab0a-1f6738c3d9ef', '78cb9c57-8e9b-494d-a331-7b123d6f9ee5'),
('741a1e59-2ce2-46ce-aed3-b3f61de7f567', 'c3ec2484-ee4f-4acc-8344-c99f25dc1c3a'),
('d1772ec6-c71b-4258-a929-afaf0f4d1b6a', '415a63e9-3b20-4967-9a46-f6fa6e35072c'),
('fbfedc6b-9ef1-42c6-85aa-69cd38842744', '78cb9c57-8e9b-494d-a331-7b123d6f9ee5');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aspnetusers`
--

CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `DateOfBirth` date DEFAULT NULL,
  `Discriminator` longtext NOT NULL,
  `Name` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `aspnetusers`
--

INSERT INTO `aspnetusers` (`Id`, `UserName`, `NormalizedUserName`, `Email`, `NormalizedEmail`, `EmailConfirmed`, `PasswordHash`, `SecurityStamp`, `ConcurrencyStamp`, `PhoneNumber`, `PhoneNumberConfirmed`, `TwoFactorEnabled`, `LockoutEnd`, `LockoutEnabled`, `AccessFailedCount`, `DateOfBirth`, `Discriminator`, `Name`) VALUES
('27a9cf8c-6f49-40e8-9b2a-7314b436105a', 'nguyencus@gmail.com', NULL, 'nbnguyen1100@gmail.com', NULL, 0, 'AQAAAAEAACcQAAAAEJH0UF8N3rj9VQ3rJjB3Kk7P+nf2guW8+BFB5CxT0ODAKIj+EfJEdhnwx1bgK+/1Vw==', 'f4e471c3-7bb6-4bd6-b428-87bb3ea2e0d6', 'a22ce0d4-dcc1-49d2-b9db-48f5c64c09db', NULL, 0, 0, NULL, 0, 0, '2023-12-22', '', 'Nguyen2'),
('4626bb53-48f9-47c5-9732-0fcf918a423a', 'Nguyen3', 'NGUYEN3', 'nguyencus@gmail.com', 'NGUYENCUS@GMAIL.COM', 0, 'Nguyen.', '6bfb80bd-edff-43d5-a798-66f357464e8d', 'cb533c78-6f61-436c-af29-71228f3018c8', '0123123123', 0, 0, NULL, 0, 0, '2023-12-06', '', 'Employee'),
('4822582d-7b0e-47a0-9361-8ad7ee02c61a', 'dấd', NULL, 'nbnguyen@gmail.com', NULL, 0, 'Nguyen.', '94280e56-8d4f-4764-a98f-47149bf7138e', '3e565e65-9403-40eb-834d-cb6ad9952ecd', '0123123123', 0, 0, NULL, 0, 0, '2023-12-13', '', 'Nguyên Driver'),
('59c09a5c-2b80-4e34-ab0a-1f6738c3d9ef', 'khai@gmail.com', 'KHAI@GMAIL.COM', 'khai@gmail.com', 'KHAI@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEIbkvrzE29H2i4leVxApmxNljlFml3IjQFJS30aVSocjsctlk3bfbWAVUO24UOLxvw==', '6D5QLZILI4KYBBUANJZ7RP4SP7LR5BJ4', '5debe327-1bbb-455d-9a8b-0b3d53b48899', NULL, 0, 0, NULL, 1, 0, '2023-12-30', 'ApplicationUser', 'Khải'),
('627c7e7d-e433-4a1d-a0fa-8c96372be8e8', 'Nguyen2', NULL, 'nbnguyen1100@gmail.com', NULL, 0, 'Nguyen.', 'b46b77fa-23fa-49cd-bfc3-8c432fe6d897', '63193700-99c5-4380-a285-7a6e89dcabed', '0123123123', 0, 0, NULL, 0, 0, '2023-12-22', '', 'Driver1'),
('741a1e59-2ce2-46ce-aed3-b3f61de7f567', 'Nguyen2', 'NGUYEN2', 'nbnguyen1100@gmail.com', 'NBNGUYEN1100@GMAIL.COM', 0, 'Nguyen.', '7371fb19-f6ce-4c8d-ad4a-42c7aa5379f9', '117031d6-2cf5-40c5-8fd9-2d5b99f53466', '0123123123', 0, 0, NULL, 0, 0, '2023-12-28', '', 'Nguyên Driver'),
('8ca09141-0c73-4e27-9306-8bf3708044db', 'dấd', NULL, 'nbnguyen1100@gmail.com', NULL, 0, 'Nguyen.', '42f2aa32-6e51-42b4-9083-63007c49b47f', 'bef51166-7df9-4fff-8f8e-e7591c4f08b8', '0123123123', 0, 0, NULL, 0, 0, '2023-12-14', '', 'Nguyên Driver'),
('c5989303-f506-4136-819f-8b074ac7a5ee', 'Nguyen2', NULL, 'nbnguyen1100@gmail.com', NULL, 0, 'asdasd', '22216f21-c5de-45ac-b585-fa2d68c62180', 'edc7da64-03a9-4b5b-9564-f82dc4353f3b', '0123123123', 0, 0, NULL, 0, 0, '2023-12-14', '', 'Nguyên Driver'),
('d1772ec6-c71b-4258-a929-afaf0f4d1b6a', 'nguyen@gmail.com', 'NGUYEN@GMAIL.COM', 'nguyen@gmail.com', 'NGUYEN@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEJKrh9T3OENlloCXOW7x76iYDUH++82PNPE735G9LFLqPrRn/88VxLvrWSATYXOxWw==', 'SN27S3JGKQCQGF45NNSAS23Z43LI5ZJY', '6069fd1e-6e9a-44fa-8420-26d9daf2d44d', '012333456', 0, 0, NULL, 1, 0, '2023-12-02', 'ApplicationUser', 'Nguyên'),
('ddecb5e2-c981-43a5-9dc4-2cf9d3cf67fa', 'Nguyen2', NULL, 'nbnguyen1100@gmail.com', NULL, 0, 'Nguyen.', '873fa255-e7b1-464d-9173-65ccbb68c2ac', 'a4997056-45fd-4591-b68a-3d1efac04d9d', '0123123123', 0, 0, NULL, 0, 0, '2023-12-10', '', 'Driver1'),
('fbfedc6b-9ef1-42c6-85aa-69cd38842744', 'nguyenemp@gmail.com', 'NGUYENEMP@GMAIL.COM', 'nguyenemp@gmail.com', 'NGUYENEMP@GMAIL.COM', 0, 'AQAAAAEAACcQAAAAEDA59BaAwz3PIvMcGQifb78QrQzmekIpi/ZNgnql66aOrnAGkHnP9W4PURaiNsfDJw==', '6V4MTL3MFPQWSKYAEIQGUNFJK7O7X4X6', '1fa54bc7-7703-4b28-a3e3-5014c33b1f09', NULL, 0, 0, NULL, 1, 0, '2023-12-13', '', 'Nguyen Emp');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `aspnetusertokens`
--

CREATE TABLE `aspnetusertokens` (
  `UserId` varchar(255) NOT NULL,
  `LoginProvider` varchar(128) NOT NULL,
  `Name` varchar(128) NOT NULL,
  `Value` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `coaches`
--

CREATE TABLE `coaches` (
  `coach_id` int(11) NOT NULL,
  `coach_no` varchar(50) DEFAULT NULL,
  `user_id` varchar(255) DEFAULT NULL,
  `supplier` varchar(50) DEFAULT NULL,
  `capacity` int(11) DEFAULT NULL,
  `status` varchar(50) DEFAULT 'Active',
  `description` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `coaches`
--

INSERT INTO `coaches` (`coach_id`, `coach_no`, `user_id`, `supplier`, `capacity`, `status`, `description`) VALUES
(3, '51A-123456', '59c09a5c-2b80-4e34-ab0a-1f6738c3d9ef', 'Toyota', 40, 'Active', 'xe 40 cho'),
(29, '79A-456789', '59c09a5c-2b80-4e34-ab0a-1f6738c3d9ef', 'Mazda', 40, 'Active', 'xe 40 cho');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `locations`
--

CREATE TABLE `locations` (
  `location_id` int(11) NOT NULL,
  `location_name` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `locations`
--

INSERT INTO `locations` (`location_id`, `location_name`) VALUES
(1, 'Hà Nội'),
(2, 'Hồ Chí Minh'),
(3, 'Đà Nẵng'),
(4, 'Hải Phòng'),
(5, 'Cần Thơ'),
(6, 'An Giang'),
(7, 'Bà Rịa-Vũng Tàu'),
(8, 'Bắc Giang'),
(9, 'Bắc Kạn'),
(10, 'Bạc Liêu'),
(11, 'Bắc Ninh'),
(12, 'Bến Tre'),
(13, 'Bình Định'),
(14, 'Bình Dương'),
(15, 'Bình Phước'),
(16, 'Bình Thuận'),
(17, 'Cà Mau'),
(18, 'Cao Bằng'),
(19, 'Đắk Lắk'),
(20, 'Đắk Nông'),
(21, 'Điện Biên'),
(22, 'Đồng Nai'),
(23, 'Đồng Tháp'),
(24, 'Gia Lai'),
(25, 'Hà Giang'),
(26, 'Hà Nam'),
(27, 'Hà Tĩnh'),
(28, 'Hải Dương'),
(29, 'Hậu Giang'),
(30, 'Hòa Bình'),
(31, 'Hưng Yên'),
(32, 'Khánh Hòa'),
(33, 'Kiên Giang'),
(34, 'Kon Tum'),
(35, 'Lai Châu'),
(36, 'Lâm Đồng'),
(37, 'Lạng Sơn'),
(38, 'Lào Cai'),
(39, 'Long An'),
(40, 'Nam Định'),
(41, 'Nghệ An'),
(42, 'Ninh Bình'),
(43, 'Ninh Thuận'),
(44, 'Phú Thọ'),
(45, 'Quảng Bình'),
(46, 'Quảng Nam'),
(47, 'Quảng Ngãi'),
(48, 'Quảng Ninh'),
(49, 'Quảng Trị'),
(50, 'Sóc Trăng'),
(51, 'Sơn La'),
(52, 'Tây Ninh'),
(53, 'Thái Bình'),
(54, 'Thái Nguyên'),
(55, 'Thanh Hóa'),
(56, 'Thừa Thiên Huế'),
(57, 'Tiền Giang'),
(58, 'Trà Vinh'),
(59, 'Tuyên Quang'),
(60, 'Vĩnh Long'),
(61, 'Vĩnh Phúc'),
(62, 'Yên Bái'),
(63, 'Nha Trang'),
(64, 'Đà Lạt'),
(66, 'đâsdasdasd');

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `pick_locations`
--

CREATE TABLE `pick_locations` (
  `pick_location_id` int(11) NOT NULL,
  `pick_location_name` varchar(50) DEFAULT NULL,
  `location_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `pick_locations`
--

INSERT INTO `pick_locations` (`pick_location_id`, `pick_location_name`, `location_id`) VALUES
INSERT INTO `pick_locations` (`pick_location_id`, `pick_location_name`, `location_id`) VALUES
(1, 'Bến xe Hải Phòng', 4),
(2, 'Bến xe Nha Trang', 32),
(3, 'Bến xe Giáp Bát', 1),
(4, 'Bến xe Mỹ Đình', 1),
(5, 'Bến xe Yên Nghĩa', 1),
(6, 'Bến xe Gia Lâm', 1),
(7, 'Bến xe Nước Ngầm', 1),
(8, 'Bến xe Lương Yên', 1),
(9, 'Bến xe Sơn Tây', 1),
(10, 'Bến xe Trôi Phùng', 1),
(11, 'Bến xe Cổ Đô', 1),
(12, 'Bến xe Miền Tây', 2),
(13, 'Bến xe Miền Đông Cũ', 2),
(14, 'Bến xe Miền Đông Mới', 2),
(15, 'Bến xe Chợ Lớn', 2),
(16, 'Bến xe An Sương', 2),
(17, 'Bến xe Củ Chi', 2),
(18, 'Bến xe Tân Phú', 2),
(19, 'Bến xe Đà Nẵng', 3),
(20, 'Bến xe Cần Thơ', 5),
(21, 'Bến xe An Giang', 3),
(22, 'Bến xe Bà Rịa-Vũng Tàu', 3),
(23, 'Bến xe Bạc Liêu', 3),
(24, 'Bến xe Bến Tre', 3),
(25, 'Bến xe Bình Định', 3),
(26, 'Bến xe Bình Dương', 3),
(27, 'Bến xe Bình Phước', 3),
(28, 'Bến xe Bình Thuận', 3),
(29, 'Bến xe Cà Mau', 3),
(30, 'Bến xe Đắk Lắk', 3),
(31, 'Bến xe Đắk Nông', 3),
(32, 'Bến xe Đồng Nai', 3),
(33, 'Bến xe Đồng Tháp', 3),
(34, 'Bến xe Gia Lai', 3),
(35, 'Bến xe Khánh Hòa', 3),
(36, 'Bến xe Long An', 3),
(37, 'Bến xe Ninh Thuận', 3),
(38, 'Bến xe Quảng Nam', 3),
(39, 'Bến xe Quảng Ngãi', 3),
(40, 'Bến xe Sóc Trăng', 3),
(41, 'Bến xe Tây Ninh', 3),
(42, 'Bến xe Thừa Thiên Huế', 3),
(43, 'Bến xe Tiền Giang', 3),
(44, 'Bến xe Trà Vinh', 3),
(45, 'Bến xe Nha Trang', 3),
(46, 'Bến xe Đà Lạt', 3);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `routes`
--

CREATE TABLE `routes` (
  `route_id` int(11) NOT NULL,
  `coach_id` int(11) DEFAULT NULL,
  `location_id1` int(11) DEFAULT NULL,
  `location_id2` int(11) DEFAULT NULL,
  `start_date` datetime DEFAULT NULL,
  `end_date` datetime DEFAULT NULL,
  `price` float DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `routes`
--

INSERT INTO `routes` (`route_id`, `coach_id`, `location_id1`, `location_id2`, `start_date`, `end_date`, `price`) VALUES
(1, 29, 2, 32, '2023-12-24 19:00:00', '2023-12-25 05:00:00', 200000);

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `tickets`
--

CREATE TABLE `tickets` (
  `invoice_id` int(11) NOT NULL,
  `user_id` varchar(255) DEFAULT NULL,
  `seat_no` int(11) DEFAULT NULL,
  `invoice_date` datetime DEFAULT current_timestamp(),
  `pick_location_id1` int(11) DEFAULT NULL,
  `pick_location_id2` int(11) DEFAULT NULL,
  `route_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Cấu trúc bảng cho bảng `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Đang đổ dữ liệu cho bảng `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20231206035624_IdentityTables', '6.0.25'),
('20231206040901_InitialDatabase', '6.0.25'),
('20231206071847_NewUserColumn', '6.0.25'),
('20231206072207_NewUserColumn', '6.0.25'),
('20231206074421_NewUserColumn', '6.0.25');

--
-- Chỉ mục cho các bảng đã đổ
--

--
-- Chỉ mục cho bảng `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`);

--
-- Chỉ mục cho bảng `aspnetroles`
--
ALTER TABLE `aspnetroles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `RoleNameIndex` (`NormalizedName`);

--
-- Chỉ mục cho bảng `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_AspNetUserClaims_UserId` (`UserId`);

--
-- Chỉ mục cho bảng `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  ADD KEY `IX_AspNetUserLogins_UserId` (`UserId`);

--
-- Chỉ mục cho bảng `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD PRIMARY KEY (`UserId`,`RoleId`),
  ADD KEY `IX_AspNetUserRoles_RoleId` (`RoleId`);

--
-- Chỉ mục cho bảng `aspnetusers`
--
ALTER TABLE `aspnetusers`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  ADD KEY `EmailIndex` (`NormalizedEmail`);

--
-- Chỉ mục cho bảng `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD PRIMARY KEY (`UserId`,`LoginProvider`,`Name`);

--
-- Chỉ mục cho bảng `coaches`
--
ALTER TABLE `coaches`
  ADD PRIMARY KEY (`coach_id`),
  ADD KEY `user_id` (`user_id`);

--
-- Chỉ mục cho bảng `locations`
--
ALTER TABLE `locations`
  ADD PRIMARY KEY (`location_id`);

--
-- Chỉ mục cho bảng `pick_locations`
--
ALTER TABLE `pick_locations`
  ADD PRIMARY KEY (`pick_location_id`),
  ADD KEY `location_id` (`location_id`);

--
-- Chỉ mục cho bảng `routes`
--
ALTER TABLE `routes`
  ADD PRIMARY KEY (`route_id`),
  ADD KEY `coach_id` (`coach_id`),
  ADD KEY `location_id1` (`location_id1`),
  ADD KEY `location_id2` (`location_id2`);

--
-- Chỉ mục cho bảng `tickets`
--
ALTER TABLE `tickets`
  ADD PRIMARY KEY (`invoice_id`),
  ADD KEY `user_id` (`user_id`),
  ADD KEY `fk_pick_location` (`pick_location_id1`),
  ADD KEY `fk_pick_location2` (`pick_location_id2`),
  ADD KEY `route_id` (`route_id`);

--
-- Chỉ mục cho bảng `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT cho các bảng đã đổ
--

--
-- AUTO_INCREMENT cho bảng `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT cho bảng `coaches`
--
ALTER TABLE `coaches`
  MODIFY `coach_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT cho bảng `locations`
--
ALTER TABLE `locations`
  MODIFY `location_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;

--
-- AUTO_INCREMENT cho bảng `pick_locations`
--
ALTER TABLE `pick_locations`
  MODIFY `pick_location_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT cho bảng `routes`
--
ALTER TABLE `routes`
  MODIFY `route_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT cho bảng `tickets`
--
ALTER TABLE `tickets`
  MODIFY `invoice_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Các ràng buộc cho các bảng đã đổ
--

--
-- Các ràng buộc cho bảng `aspnetroleclaims`
--
ALTER TABLE `aspnetroleclaims`
  ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `aspnetuserclaims`
--
ALTER TABLE `aspnetuserclaims`
  ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `aspnetuserlogins`
--
ALTER TABLE `aspnetuserlogins`
  ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `aspnetuserroles`
--
ALTER TABLE `aspnetuserroles`
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `aspnetusertokens`
--
ALTER TABLE `aspnetusertokens`
  ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE;

--
-- Các ràng buộc cho bảng `coaches`
--
ALTER TABLE `coaches`
  ADD CONSTRAINT `fk_userid` FOREIGN KEY (`user_id`) REFERENCES `aspnetusers` (`Id`);

--
-- Các ràng buộc cho bảng `pick_locations`
--
ALTER TABLE `pick_locations`
  ADD CONSTRAINT `pick_locations_ibfk_1` FOREIGN KEY (`location_id`) REFERENCES `locations` (`location_id`);

--
-- Các ràng buộc cho bảng `routes`
--
ALTER TABLE `routes`
  ADD CONSTRAINT `routes_ibfk_1` FOREIGN KEY (`coach_id`) REFERENCES `coaches` (`coach_id`),
  ADD CONSTRAINT `routes_ibfk_2` FOREIGN KEY (`location_id1`) REFERENCES `locations` (`location_id`),
  ADD CONSTRAINT `routes_ibfk_3` FOREIGN KEY (`location_id2`) REFERENCES `locations` (`location_id`);

--
-- Các ràng buộc cho bảng `tickets`
--
ALTER TABLE `tickets`
  ADD CONSTRAINT `fk_pick_location` FOREIGN KEY (`pick_location_id1`) REFERENCES `pick_locations` (`pick_location_id`),
  ADD CONSTRAINT `fk_pick_location2` FOREIGN KEY (`pick_location_id2`) REFERENCES `pick_locations` (`pick_location_id`),
  ADD CONSTRAINT `fk_userid_invoices` FOREIGN KEY (`user_id`) REFERENCES `aspnetusers` (`Id`),
  ADD CONSTRAINT `tickets_ibfk_1` FOREIGN KEY (`route_id`) REFERENCES `routes` (`route_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
