USE [DBThoiTrang]
GO

INSERT [dbo].[UserGroups] ([GroupId], [Name]) VALUES 
(N'ADMIN', N'Quản trị'),
(N'MOD', N'Quản lý'),
(N'USER', N'Người dùng')
GO

INSERT [dbo].[Role] ([RoleId], [Name]) VALUES 
(N'ADD_ADMIN', N'thêm quản trị'),
(N'ADD_CATE', N'thêm loại sản phẩm'),
(N'ADD_NEWS', N'Thêm tin tức'),
(N'ADD_PRODUCT', N'thêm sản phẩm'),
(N'ADD_PROVIDER', N'thêm nhà cung cấp'),
(N'ADD_USER', N'thêm người dùng'),
(N'DELETE_ADMIN', N'xóa quản trị'),
(N'DELETE_CATE', N'xóa loại sản phẩm'),
(N'DELETE_NEWS', N'xóa tin tức'),
(N'DELETE_PRODUCT', N'xóa sản phẩm'),
(N'DELETE_PROVIDER', N'xóa nhà cung cấp'),
(N'DELETE_USER', N'xóa người dùng'),
(N'MANAGER_ORDER', N'quản lý đơn hàng'),
(N'PRINT_ORDER', N'in đơn hàng'),
(N'SATISTIC_ORDER', N'thống kê đơn hàng'),
(N'SHOW_ORDER', N'xem đơn hàng'),
(N'UPDATE_ADMIN', N'cập nhật quản trị'),
(N'UPDATE_CATE', N'cập nhật loại sản phẩm'),
(N'UPDATE_NEWS', N'Cập nhật tin tức'),
(N'UPDATE_PRODUCT', N'cập nhật sản phẩm'),
(N'UPDATE_PROVIDER', N'cập nhật nhà cung cấp'),
(N'UPDATE_USER', N'cập nhật người dùng'),
(N'VIEW_ADMIN', N'hiển thị quản trị'),
(N'VIEW_CATE', N'hiển thị loại sản phẩm'),
(N'VIEW_NEWS', N'hiển thị tin tức'),
(N'VIEW_PRODUCT', N'hiển thị sản phẩm'),
(N'VIEW_PROVIDER', N'hiển thị nhà cung cấp'),
(N'VIEW_USER', N'hiển thị người dùng')
GO

-- Credentials data
SET IDENTITY_INSERT [dbo].[Credentials] ON 
GO
INSERT [dbo].[Credentials] ([CredenId], [UserGroupId], [RoleId]) VALUES 
-- ADMIN permissions
(1, N'ADMIN', N'ADD_ADMIN'),
(2, N'ADMIN', N'ADD_CATE'), 
(3, N'ADMIN', N'ADD_NEWS'),
(4, N'ADMIN', N'ADD_PRODUCT'),
(5, N'ADMIN', N'ADD_PROVIDER'),
(6, N'ADMIN', N'ADD_USER'),
(7, N'ADMIN', N'DELETE_ADMIN'),
(8, N'ADMIN', N'DELETE_CATE'),
(9, N'ADMIN', N'DELETE_NEWS'),
(10, N'ADMIN', N'DELETE_PRODUCT'),
(11, N'ADMIN', N'DELETE_PROVIDER'),
(12, N'ADMIN', N'DELETE_USER'),
(13, N'ADMIN', N'MANAGER_ORDER'),
(14, N'ADMIN', N'PRINT_ORDER'),
(15, N'ADMIN', N'SATISTIC_ORDER'),
(16, N'ADMIN', N'SHOW_ORDER'),
(17, N'ADMIN', N'UPDATE_ADMIN'),
(18, N'ADMIN', N'UPDATE_CATE'),
(19, N'ADMIN', N'UPDATE_NEWS'),
(20, N'ADMIN', N'UPDATE_PRODUCT'),
(21, N'ADMIN', N'UPDATE_PROVIDER'),
(22, N'ADMIN', N'UPDATE_USER'),
(23, N'ADMIN', N'VIEW_ADMIN'),
(24, N'ADMIN', N'VIEW_CATE'),
(25, N'ADMIN', N'VIEW_NEWS'),
(26, N'ADMIN', N'VIEW_PRODUCT'),
(27, N'ADMIN', N'VIEW_PROVIDER'),
(28, N'ADMIN', N'VIEW_USER'),

-- MOD permissions
(29, N'MOD', N'UPDATE_ADMIN'),
(30, N'MOD', N'UPDATE_USER'),
(31, N'MOD', N'VIEW_ADMIN'),
(32, N'MOD', N'VIEW_CATE'),
(33, N'MOD', N'VIEW_NEWS'),
(34, N'MOD', N'VIEW_PRODUCT'),
(35, N'MOD', N'VIEW_PROVIDER'),
(36, N'MOD', N'VIEW_USER'),
(37, N'MOD', N'MANAGER_ORDER'),
(38, N'MOD', N'PRINT_ORDER'),
(39, N'MOD', N'SATISTIC_ORDER'),
(40, N'MOD', N'SHOW_ORDER')
GO
SET IDENTITY_INSERT [dbo].[Credentials] OFF
GO

SET IDENTITY_INSERT [dbo].[User] ON 
GO
INSERT [dbo].[User] ([UserId], [Name], [Address], [Phone], [Username], [Password], [Email], [GroupId], [Status]) VALUES 
(39, N'Nguyễn Văn An', N'15 Trần Phú, Hà Đông, Hà Nội', 0987654321, N'nguyenvanan', N'332532dcfaa1cbf61e2a266bd723612c', N'nguyenvanan@gmail.com', N'USER', 1),
(40, N'Admin System', N'25 Lê Lợi, Hoàn Kiếm, Hà Nội', 0912345678, N'admin', N'21232f297a57a5a743894a0e4a801fc3', N'admin@fashionshop.com', N'ADMIN', 1),
(41, N'Trần Thị Bình', N'35 Nguyễn Huệ, Q1, TP.HCM', 0965432109, N'tranthiminh', N'498d3c6bfa033f6dc1be4fcc3c370aa7', N'tranthiminh@gmail.com', N'USER', 1),
(42, N'Lê Văn Cường', N'45 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', 0943215678, N'levancuong', N'fcf321d09609565b7a1ce6e70f1f5056', N'levancuong@gmail.com', N'MOD', 1),
(43, N'Phạm Thị Dung', N'55 Lý Thường Kiệt, Hai Bà Trưng, Hà Nội', 0932165478, N'phamthidung', N'e10adc3949ba59abbe56e057f20f883e', N'phamthidung@gmail.com', N'USER', 1),
(44, N'Hoàng Văn Em', N'65 Bà Triệu, Hoàn Kiếm, Hà Nội', 0954321678, N'hoangvanem', N'fcea920f7412b5da7be0cf42b8c93759', N'hoangvanem@gmail.com', N'USER', 1),
(45, N'Ngô Thị Phương', N'75 Quang Trung, Gò Vấp, TP.HCM', 0987612345, N'ngothiphuong', N'25d55ad283aa400af464c76d713c07ad', N'ngothiphuong@gmail.com', N'MOD', 1),
(46, N'Đỗ Văn Giang', N'85 Nguyễn Trãi, Thanh Xuân, Hà Nội', 0912378945, N'dovangiang', N'e807f1fcf82d132f9bb018ca6738a19f', N'dovangiang@gmail.com', N'USER', 1),
(47, N'Vũ Thị Hương', N'95 Lê Duẩn, Đống Đa, Hà Nội', 0965438901, N'vuthihuong', N'1234567890abcdef1234567890abcdef', N'vuthihuong@gmail.com', N'USER', 1),
(48, N'Bùi Văn Kiên', N'105 Giải Phóng, Hai Bà Trưng, Hà Nội', 0943217890, N'buivankien', N'abcdef1234567890abcdef1234567890', N'buivankien@gmail.com', N'USER', 1)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO

-- Card data
SET IDENTITY_INSERT [dbo].[Card] ON 
GO
INSERT [dbo].[Card] ([CardId], [NumberCard], [UserId]) VALUES 
(1, 560, 40),
(2, 5600, 39),
(3, 0, 41)
GO
SET IDENTITY_INSERT [dbo].[Card] OFF
GO

-- Category data
INSERT [dbo].[Category] ([CategoryId], [Name], [MetaTitle], [ParId]) VALUES 
(1, N'Thời trang nam', N'thoi-trang-nam', 0),
(2, N'Thời trang nữ', N'thoi-trang-nu', 0),
(3, N'Phụ kiện', N'phu-kien', 0),
(4, N'Giày dép', N'giay-dep', 0),
(5, N'Áo nam', N'ao-nam', 1),
(6, N'Quần nam', N'quan-nam', 1),
(7, N'Áo nữ', N'ao-nu', 2),
(8, N'Quần nữ', N'quan-nu', 2),
(9, N'Váy đầm', N'vay-dam', 2),
(10, N'Túi xách', N'tui-xach', 3),
(11, N'Ví bóp', N'vi-bop', 3),
(12, N'Thắt lưng', N'that-lung', 3),
(13, N'Giày nam', N'giay-nam', 4),
(14, N'Giày nữ', N'giay-nu', 4),
(15, N'Dép nam', N'dep-nam', 4),
(16, N'Dép nữ', N'dep-nu', 4)
GO

SET IDENTITY_INSERT [dbo].[Provider] ON 
GO
INSERT [dbo].[Provider] ([ProviderId], [Name], [Phone], [Address]) VALUES
(1, N'Công ty thời trang ABC', '0987654321', N'Hà Nội'),
(2, N'Xưởng may XYZ', '0912345678', N'TP.HCM'),
(3, N'Nhà máy dệt may 123', '0965432109', N'Hải Phòng')
GO
SET IDENTITY_INSERT [dbo].[Provider] OFF
GO

SET IDENTITY_INSERT [dbo].[Status] ON 
GO
INSERT [dbo].[Status] ([StatusId], [Name]) VALUES 
(1, N'Đã tiếp nhận'),
(2, N'Đang chờ xử lý'),
(3, N'Đã xử lý'),
(4, N'Đã hoàn thành')
GO
SET IDENTITY_INSERT [dbo].[Status] OFF
GO

SET IDENTITY_INSERT [dbo].[Product] ON 
GO
INSERT INTO [dbo].[Product] ([ProductId], [Name], [Description], [Price], [Quantity], [ProviderId], [CateId], [Photo], [StartDate], [EndDate], [Discount], [IsHidden]) VALUES
-- Áo Nam (CateId: 5)
(1, N'Áo Thun Basic Nam', N'Áo thun cotton 100% form regular fit', 2500000, 150, 1, 5, '1.jpg', NULL, NULL, NULL, 0),
(2, N'Áo Polo Nam Premium', N'Áo polo cotton cao cấp', 4500000, 120, 2, 5, '2.jpg', '2024-01-01', '2024-01-15', 15, 0),
(3, N'Áo Sơ Mi Nam Công Sở', N'Áo sơ mi dài tay chất liệu lụa', 550000, 100, 3, 5, '3.jpg', NULL, NULL, NULL, 0),
(4, N'Áo Hoodie Nam Nỉ', N'Áo hoodie nỉ bông dày dặn', 6500000, 80, 1, 5, '4.jpg', '2024-01-05', '2024-01-20', 20, 0),
(5, N'Áo Khoác Jean Nam', N'Áo khoác jean wash rách', 7500000, 90, 2, 5, '5.jpg', NULL, NULL, NULL, 0),
(6, N'Áo Blazer Nam Hàn Quốc', N'Áo blazer form rộng', 8500000, 70, 3, 5, '6.jpg', '2024-01-10', '2024-01-25', 25, 0),
(7, N'Áo Len Nam Cổ Lọ', N'Áo len nam dệt kim', 4500000, 110, 1, 5, '7.jpg', NULL, NULL, NULL, 0),
(8, N'Áo Thun Nam In Họa Tiết', N'Áo thun cotton in hình', 3500000, 130, 2, 5, '8.jpg', '2024-01-15', '2024-01-30', 10, 0),

-- Quần Nam (CateId: 6)
(9, N'Quần Jean Nam Skinny', N'Quần jean ôm body', 6500000, 100, 3, 6, '9.jpg', NULL, NULL, NULL, 0),
(10, N'Quần Tây Nam Công Sở', N'Quần âu nam đen', 5500000, 90, 1, 6, '10.jpg', '2024-01-20', '2024-02-05', 15, 0),
(11, N'Quần Kaki Nam', N'Quần kaki nam form regular', 4500000, 120, 2, 6, '11.jpg', NULL, NULL, NULL, 0),
(12, N'Quần Short Jean Nam', N'Quần short jean basic', 3500000, 150, 3, 6, '12.jpg', '2024-01-25', '2024-02-10', 20, 0),
(13, N'Quần Jogger Nam', N'Quần jogger thể thao', 4000000, 130, 1, 6, '13.jpg', NULL, NULL, NULL, 0),
(14, N'Quần Baggy Nam', N'Quần baggy nam rộng', 5000000, 110, 2, 6, '14.jpg', '2024-02-01', '2024-02-15', 25, 0),

-- Áo Nữ (CateId: 7)
(15, N'Áo Kiểu Nữ', N'Áo kiểu nữ công sở', 3500000, 120, 3, 7, '15.jpg', NULL, NULL, NULL, 0),
(16, N'Áo Croptop Nữ', N'Áo croptop nữ basic', 2500000, 150, 1, 7, '16.jpg', '2024-02-05', '2024-02-20', 15, 0),
(17, N'Áo Sơ Mi Nữ', N'Áo sơ mi nữ tay dài', 4500000, 100, 2, 7, '17.jpg', NULL, NULL, NULL, 0),
(18, N'Áo Thun Nữ Form Rộng', N'Áo thun nữ oversize', 3000000, 140, 3, 7, '18.jpg', '2024-02-10', '2024-02-25', 20, 0),
(19, N'Áo Len Nữ Crop', N'Áo len nữ ngắn', 4000000, 110, 1, 7, '19.jpg', NULL, NULL, NULL, 0),
(20, N'Áo Hai Dây', N'Áo hai dây lụa', 3500000, 130, 2, 7, '20.jpg', '2024-02-15', '2024-03-01', 25, 0),

-- Quần Nữ (CateId: 8)
(21, N'Quần Jean Nữ Ống Rộng', N'Quần jean nữ form rộng', 5500000, 100, 3, 8, '21.jpg', NULL, NULL, NULL, 0),
(22, N'Quần Culottes Nữ', N'Quần culottes công sở', 4500000, 90, 1, 8, '22.jpg', '2024-02-20', '2024-03-05', 15, 0),
(23, N'Quần Short Nữ', N'Quần short nữ basic', 3500000, 120, 2, 8, '23.jpg', NULL, NULL, NULL, 0),
(24, N'Quần Baggy Nữ', N'Quần baggy nữ kẻ caro', 5000000, 80, 3, 8, '24.jpg', '2024-02-25', '2024-03-10', 20, 0),
(25, N'Quần Legging Nữ', N'Quần legging nữ tập gym', 3000000, 150, 1, 8, '25.jpg', NULL, NULL, NULL, 0),

-- Váy Đầm (CateId: 9)
(26, N'Váy Suông Công Sở', N'Váy suông thanh lịch', 6500000, 80, 2, 9, '26.jpg', '2024-03-01', '2024-03-15', 25, 0),
(27, N'Đầm Maxi Dự Tiệc', N'Đầm maxi dài sang trọng', 8500000, 60, 3, 9, '27.jpg', NULL, NULL, NULL, 0),
(28, N'Váy Tennis', N'Váy tennis xếp ly', 4000000, 100, 1, 9, '28.jpg', '2024-03-05', '2024-03-20', 15, 0),
(29, N'Đầm Body', N'Đầm body ôm sát', 5500000, 70, 2, 9, '29.jpg', NULL, NULL, NULL, 0),
(30, N'Váy Babydoll', N'Váy babydoll cute', 4500000, 90, 3, 9, '30.jpg', '2024-03-10', '2024-03-25', 20, 0),

-- Túi Xách (CateId: 10)
(31, N'Túi Xách Tay Nữ', N'Túi xách tay da cao cấp', 12000000, 50, 1, 10, '31.jpg', NULL, NULL, NULL, 0),
(32, N'Túi Đeo Chéo', N'Túi đeo chéo nhỏ gọn', 8000000, 70, 2, 10, '32.jpg', '2024-03-15', '2024-03-30', 25, 0),
(33, N'Túi Tote', N'Túi tote canvas', 4000000, 100, 3, 10, '33.jpg', NULL, NULL, NULL, 0),
(34, N'Túi Clutch', N'Túi clutch dự tiệc', 6000000, 60, 1, 10, '34.jpg', '2024-03-20', '2024-04-05', 15, 0),
(35, N'Balo Thời Trang', N'Balo da thời trang', 9000000, 40, 2, 10, '35.jpg', NULL, NULL, NULL, 0),

-- Ví Bóp (CateId: 11)
(36, N'Ví Dài Nam', N'Ví da nam cao cấp', 5000000, 80, 3, 11, '36.jpg', '2024-03-25', '2024-04-10', 20, 0),
(37, N'Ví Ngắn Nữ', N'Ví nữ mini', 4000000, 100, 1, 11, '37.jpg', NULL, NULL, NULL, 0),
(38, N'Ví Card', N'Ví đựng card nhỏ gọn', 2000000, 150, 2, 11, '38.jpg', '2024-04-01', '2024-04-15', 25, 0),
(39, N'Ví Cầm Tay Nữ', N'Ví cầm tay nữ thời trang', 6000000, 70, 3, 11, '39.jpg', NULL, NULL, NULL, 0),
(40, N'Ví Nam Ngang', N'Ví nam da bò', 4500000, 90, 1, 11, '40.jpg', '2024-04-05', '2024-04-20', 15, 0),

-- Thắt Lưng (CateId: 12)
(41, N'Thắt Lưng Nam Da', N'Thắt lưng da bò thật', 6000000, 70, 2, 12, '41.jpg', NULL, NULL, NULL, 0),
(42, N'Thắt Lưng Nữ Bản Nhỏ', N'Thắt lưng nữ thời trang', 4000000, 90, 3, 12, '42.jpg', '2024-04-10', '2024-04-25', 20, 0),
(43, N'Dây Nịt Nam Cao Cấp', N'Dây nịt nam khóa tự động', 8000000, 50, 1, 12, '43.jpg', NULL, NULL, NULL, 0),
(44, N'Thắt Lưng Vải', N'Thắt lưng vải canvas', 2500000, 120, 2, 12, '44.jpg', '2024-04-15', '2024-04-30', 25, 0),
(45, N'Thắt Lưng Da Đan', N'Thắt lưng da đan thủ công', 7000000, 60, 3, 12, '45.jpg', NULL, NULL, NULL, 0),

-- Giày Nam (CateId: 13)
(46, N'Giày Tây Nam', N'Giày tây nam da bò', 15000000, 40, 1, 13, '46.jpg', '2024-04-20', '2024-05-05', 15, 0),
(47, N'Giày Thể Thao Nam', N'Giày sneaker nam', 9000000, 80, 2, 13, '47.jpg', NULL, NULL, NULL, 0),
(48, N'Giày Lười Nam', N'Giày lười nam da thật', 8000000, 70, 3, 13, '48.jpg', '2024-04-25', '2024-05-10', 20, 0),
(49, N'Giày Boot Nam', N'Giày boot nam cao cổ', 12000000, 50, 1, 13, '49.jpg', NULL, NULL, NULL, 0),
(50, N'Giày Sandal Nam', N'Giày sandal nam da', 6000000, 90, 2, 13, '50.jpg', '2024-05-01', '2024-05-15', 25, 0),

-- Giày Nữ (CateId: 14)
(51, N'Giày Cao Gót', N'Giày cao gót 7cm', 8000000, 60, 3, 14, '51.jpg', NULL, NULL, NULL, 0),
(52, N'Giày Búp Bê', N'Giày búp bê nữ', 4000000, 100, 1, 14, '52.jpg', '2024-05-05', '2024-05-20', 15, 0),
(53, N'Giày Thể Thao Nữ', N'Giày sneaker nữ', 7000000, 80, 2, 14, '53.jpg', NULL, NULL, NULL, 0),
(54, N'Giày Oxford Nữ', N'Giày oxford nữ da', 9000000, 50, 3, 14, '54.jpg', '2024-05-10', '2024-05-25', 20, 0),
(55, N'Giày Sandal Nữ', N'Giày sandal nữ đế xuồng', 5000000, 90, 1, 14, '55.jpg', NULL, NULL, NULL, 0),

-- Dép Nam (CateId: 15)
(56, N'Dép Nam Quai Ngang', N'Dép nam da bò', 4000000, 100, 2, 15, '56.jpg', '2024-05-15', '2024-05-30', 25, 0),
(57, N'Dép Nam Kẹp', N'Dép xỏ ngón nam', 3000000, 120, 3, 15, '57.jpg', NULL, NULL, NULL, 0),
(58, N'Dép Nam Thể Thao', N'Dép thể thao nam', 4500000, 90, 1, 15, '58.jpg', '2024-05-20', '2024-06-05', 15, 0),
(59, N'Dép Nam Đi Trong Nhà', N'Dép đi trong nhà nam', 2000000, 150, 2, 15, '59.jpg', NULL, NULL, NULL, 0),
(60, N'Dép Nam Cao Su', N'Dép cao su nam', 1500000, 200, 3, 15, '60.jpg', '2024-05-25', '2024-06-10', 20, 0),

-- Dép Nữ (CateId: 16)
(61, N'Dép Nữ Quai Ngang', N'Dép nữ thời trang', 3500000, 110, 1, 16, '61.jpg', NULL, NULL, NULL, 0),
(62, N'Dép Nữ Đế Xuồng', N'Dép đế xuồng nữ', 5000000, 80, 2, 16, '62.jpg', '2024-06-01', '2024-06-15', 25, 0),
(63, N'Dép Nữ Kẹp', N'Dép xỏ ngón nữ', 2500000, 130, 3, 16, '63.jpg', NULL, NULL, NULL, 0),
(64, N'Dép Nữ Đi Trong Nhà', N'Dép đi trong nhà nữ', 1800000, 160, 1, 16, '64.jpg', '2024-06-05', '2024-06-20', 15, 0),
(65, N'Dép Nữ Thời Trang', N'Dép nữ kiểu dáng mới', 4000000, 90, 2, 16, '65.jpg', NULL, NULL, NULL, 0),

-- Thêm các sản phẩm Áo Nam (CateId: 5)
(66, N'Áo Tank Top Nam', N'Áo tank top thể thao nam', 2000000, 140, 3, 5, '66.jpg', '2024-06-10', '2024-06-25', 20, 0),
(67, N'Áo Gile Nam', N'Áo gile nam công sở', 6000000, 70, 1, 5, '67.jpg', NULL, NULL, NULL, 0),
(68, N'Áo Thun Nam Cổ Tròn', N'Áo thun nam basic', 2800000, 120, 2, 5, '68.jpg', '2024-06-15', '2024-06-30', 25, 0),
(69, N'Áo Cardigan Nam', N'Áo cardigan nam len', 5500000, 80, 3, 5, '69.jpg', NULL, NULL, NULL, 0),
(70, N'Áo Khoác Dù Nam', N'Áo khoác dù nam chống nước', 4500000, 100, 1, 5, '70.jpg', '2024-06-20', '2024-07-05', 15, 0),

-- Thêm các sản phẩm Quần Nam (CateId: 6)
(71, N'Quần Short Kaki Nam', N'Quần short kaki nam basic', 3800000, 110, 2, 6, '71.jpg', NULL, NULL, NULL, 0),
(72, N'Quần Tây Nam Kẻ Sọc', N'Quần tây nam văn phòng', 6000000, 75, 3, 6, '72.jpg', '2024-06-25', '2024-07-10', 20, 0),
(73, N'Quần Jean Nam Rách', N'Quần jean nam rách gối', 5200000, 90, 1, 6, '73.jpg', NULL, NULL, NULL, 0),
(74, N'Quần Thể Thao Nam', N'Quần thể thao nam dài', 3500000, 130, 2, 6, '74.jpg', '2024-07-01', '2024-07-15', 25, 0),
(75, N'Quần Lót Nam Boxer', N'Quần lót nam boxer cotton', 1500000, 200, 3, 6, '75.jpg', NULL, NULL, NULL, 0),

-- Thêm các sản phẩm Áo Nữ (CateId: 7)
(76, N'Áo Thun Nữ Crop', N'Áo thun crop top nữ', 2500000, 140, 1, 7, '76.jpg', '2024-07-05', '2024-07-20', 15, 0),
(77, N'Áo Sơ Mi Nữ Caro', N'Áo sơ mi nữ kẻ caro', 4200000, 95, 2, 7, '77.jpg', NULL, NULL, NULL, 0),
(78, N'Áo Kiểu Nữ Tay Phồng', N'Áo kiểu nữ công sở', 3800000, 110, 3, 7, '78.jpg', '2024-07-10', '2024-07-25', 20, 0),
(79, N'Áo Len Nữ Cổ V', N'Áo len nữ dệt kim', 4500000, 85, 1, 7, '79.jpg', NULL, NULL, NULL, 0),
(80, N'Áo Thun Nữ In Hình', N'Áo thun nữ họa tiết', 2800000, 125, 2, 7, '80.jpg', '2024-07-15', '2024-07-30', 25, 0),

-- Thêm các sản phẩm Quần Nữ (CateId: 8)
(81, N'Quần Jean Nữ Skinny', N'Quần jean nữ ôm sát', 4800000, 100, 3, 8, '81.jpg', NULL, NULL, NULL, 0),
(82, N'Quần Tây Nữ Công Sở', N'Quần tây nữ đen', 5200000, 85, 1, 8, '81.jpg', '2024-07-20', '2024-08-05', 15, 0),
(83, N'Quần Short Jean Nữ', N'Quần short jean nữ basic', 3200000, 130, 2, 8, '83.jpg', NULL, NULL, NULL, 0),
(84, N'Quần Ống Rộng Nữ', N'Quần cullotes nữ kẻ sọc', 4500000, 95, 3, 8, '84.jpg', '2024-07-25', '2024-08-10', 20, 0),
(85, N'Quần Lót Nữ Cotton', N'Quần lót nữ cotton mềm mại', 1200000, 250, 1, 8, '85.jpg', NULL, NULL, NULL, 0),

-- Thêm các sản phẩm Váy Đầm (CateId: 9)
(86, N'Váy Liền Công Sở', N'Váy liền thân công sở', 5800000, 75, 2, 9, '86.jpg', '2024-07-30', '2024-08-15', 25, 0),
(87, N'Đầm Xòe Dự Tiệc', N'Đầm xòe dự tiệc sang trọng', 7500000, 60, 3, 9, '87.jpg', NULL, NULL, NULL, 0),
(88, N'Váy Chữ A', N'Váy chữ A basic', 4200000, 95, 1, 9, '88.jpg', '2024-08-05', '2024-08-20', 15, 0),
(89, N'Đầm Ôm Body', N'Đầm ôm body sexy', 5800000, 70, 2, 9, '89.jpg', NULL, NULL, NULL, 0),
(90, N'Váy Hai Dây', N'Váy hai dây dự tiệc', 4800000, 85, 3, 9, '90.jpg', '2024-08-10', '2024-08-25', 20, 0),

-- Thêm các sản phẩm Túi Xách (CateId: 10)
(91, N'Túi Xách Công Sở Nữ', N'Túi xách da cao cấp', 9800000, 55, 1, 10, '91.jpg', NULL, NULL, NULL, 0),
(92, N'Túi Đeo Chéo Mini', N'Túi đeo chéo nhỏ xinh', 5800000, 85, 2, 10, '92.jpg', '2024-08-15', '2024-08-30', 25, 0),
(93, N'Túi Canvas', N'Túi canvas đi học', 3500000, 120, 3, 10, '93.jpg', NULL, NULL, NULL, 0),
(94, N'Túi Xách Da Thật', N'Túi xách da thật cao cấp', 15000000, 45, 1, 10, '94.jpg', '2024-08-20', '2024-09-05', 15, 0),
(95, N'Túi Đeo Vai Nữ', N'Túi đeo vai thời trang', 6800000, 75, 2, 10, '95.jpg', NULL, NULL, NULL, 0),

-- Thêm các sản phẩm Ví Bóp (CateId: 11)
(96, N'Ví Da Nam Cao Cấp', N'Ví da nam nhập khẩu', 8500000, 60, 3, 11, '96.jpg', '2024-08-25', '2024-09-10', 20, 0),
(97, N'Ví Cầm Tay Nữ', N'Ví cầm tay nữ thời trang', 4500000, 90, 1, 11, '97.jpg', NULL, NULL, NULL, 0),
(98, N'Ví Mini Đựng Card', N'Ví mini đựng card tiện lợi', 2500000, 150, 2, 11, '98.jpg', '2024-09-01', '2024-09-15', 25, 0),
(99, N'Ví Dài Nữ Da Mềm', N'Ví dài nữ nhiều ngăn', 5800000, 75, 3, 11, '99.jpg', NULL, NULL, NULL, 0),
(100, N'Ví Nam Ngang Classic', N'Ví nam da bò thật', 6800000, 70, 1, 11, '100.jpg', '2024-09-05', '2024-09-20', 15, 0),

-- Thêm các sản phẩm Thắt Lưng (CateId: 12)
(101, N'Thắt Lưng Da Cao Cấp', N'Thắt lưng da thật', 7800000, 65, 2, 12, '101.jpg', NULL, NULL, NULL, 0),
(102, N'Dây Nịt Nữ Bản Nhỏ', N'Dây nịt nữ thời trang', 3500000, 95, 3, 12, '102.jpg', '2024-09-10', '2024-09-25', 20, 0),
(103, N'Thắt Lưng Nam Tự Động', N'Thắt lưng khóa tự động', 5800000, 75, 1, 12, '103.jpg', NULL, NULL, NULL, 0),
(104, N'Dây Nịt Canvas', N'Dây nịt vải bền đẹp', 2800000, 120, 2, 12, '104.jpg', '2024-09-15', '2024-09-30', 25, 0),
(105, N'Thắt Lưng Nữ Đính Kim', N'Thắt lưng nữ thời trang', 4500000, 85, 3, 12, '105.jpg', NULL, NULL, NULL, 0),

-- Thêm các sản phẩm Giày Nam (CateId: 13)
(106, N'Giày Tây Nam Công Sở', N'Giày tây nam da bóng', 12000000, 55, 1, 13, '106.jpg', '2024-09-20', '2024-10-05', 15, 0),
(107, N'Giày Thể Thao Nam Nike', N'Giày thể thao nam cao cấp', 25000000, 45, 2, 13, '107.jpg', NULL, NULL, NULL, 0),
(108, N'Giày Lười Nam', N'Giày lười nam da thật', 8500000, 70, 3, 13, '108.jpg', '2024-09-25', '2024-10-10', 20, 0),
(109, N'Giày Boot Nam', N'Giày boot nam cổ cao', 15000000, 50, 1, 13, '109.jpg', NULL, NULL, NULL, 0),
(110, N'Giày Sandal Nam', N'Giày sandal nam da bò', 6500000, 85, 2, 13, '110.jpg', '2024-10-01', '2024-10-15', 25, 0),

-- Thêm các sản phẩm Giày Nữ (CateId: 14)
(111, N'Giày Cao Gót Nữ 9cm', N'Giày cao gót nữ mũi nhọn', 7500000, 65, 3, 14, '111.jpg', NULL, NULL, NULL, 0),
(112, N'Giày Thể Thao Nữ Adidas', N'Giày thể thao nữ nhẹ nhàng', 18000000, 50, 1, 14, '112.jpg', '2024-10-05', '2024-10-20', 15, 0),
(113, N'Giày Búp Bê Nữ', N'Giày búp bê nữ công sở', 4500000, 90, 2, 14, '113.jpg', NULL, NULL, NULL, 0),
(114, N'Giày Oxford Nữ', N'Giày oxford nữ da thật', 9800000, 60, 3, 14, '114.jpg', '2024-10-10', '2024-10-25', 20, 0)
GO

SET IDENTITY_INSERT [dbo].[Product] OFF
GO

-- Order data
SET IDENTITY_INSERT [dbo].[Order] ON 
GO
INSERT [dbo].[Order] ([OrderId], [ShipName], [UserId], [ShipPhone], [ShipEmail], [UpdateDate], [ShipAddress], [StatusId]) VALUES
(1, N'Nguyễn Văn An', 39, 0987654321, N'nguyenvanan@gmail.com', CAST(N'2024-01-01T10:15:00.000' AS DateTime), N'Số 15 Trần Phú, Hà Đông, Hà Nội', 3),
(2, N'Trần Thị Bình', 40, 0912345678, N'tranthibinh@gmail.com', CAST(N'2024-01-02T14:30:00.000' AS DateTime), N'Số 25 Lê Lợi, Ngô Quyền, Hải Phòng', 4),
(3, N'Lê Văn Cường', 41, 0965432109, N'levancuong@gmail.com', CAST(N'2024-01-03T09:45:00.000' AS DateTime), N'Số 35 Nguyễn Huệ, Q1, TP.HCM', 1),
(4, N'Phạm Thị Dung', 42, 0943215678, N'phamthidung@gmail.com', CAST(N'2024-01-04T16:20:00.000' AS DateTime), N'Số 45 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', 4),
(5, N'Hoàng Văn Em', 43, 0932165478, N'hoangvanem@gmail.com', CAST(N'2024-01-05T11:30:00.000' AS DateTime), N'Số 55 Lý Thường Kiệt, Hai Bà Trưng, Hà Nội', 1),
(6, N'Ngô Thị Phương', 44, 0954321678, N'ngothiphuong@gmail.com', CAST(N'2024-01-06T13:45:00.000' AS DateTime), N'Số 65 Bà Triệu, Hoàn Kiếm, Hà Nội', 3),
(7, N'Đỗ Văn Giang', 45, 0987612345, N'dovangiang@gmail.com', CAST(N'2024-01-07T15:10:00.000' AS DateTime), N'Số 75 Quang Trung, Gò Vấp, TP.HCM', 1),
(8, N'Vũ Thị Hương', 46, 0912378945, N'vuthihuong@gmail.com', CAST(N'2024-01-08T10:25:00.000' AS DateTime), N'Số 85 Nguyễn Trãi, Thanh Xuân, Hà Nội', 1),
(9, N'Bùi Văn Kiên', 47, 0965438901, N'buivankien@gmail.com', CAST(N'2024-01-09T14:50:00.000' AS DateTime), N'Số 95 Lê Duẩn, Đống Đa, Hà Nội', 1),
(10, N'Trần Thị Lan', 48, 0943217890, N'tranthilan@gmail.com', CAST(N'2024-01-10T12:15:00.000' AS DateTime), N'Số 105 Giải Phóng, Hai Bà Trưng, Hà Nội', 1)
GO
SET IDENTITY_INSERT [dbo].[Order] OFF
GO

-- OrderDetail data
SET IDENTITY_INSERT [dbo].[OrderDetail] ON 
GO
INSERT [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Price], [Quantity]) VALUES
(1, 1, 1, 2500000, 2),  -- Áo Thun Basic Nam
(2, 1, 9, 6500000, 1),  -- Quần Jean Nam Skinny
(3, 2, 15, 3500000, 1), -- Áo Kiểu Nữ
(4, 2, 21, 5500000, 1), -- Quần Jean Nữ Ống Rộng
(5, 3, 26, 6500000, 1), -- Váy Suông Công Sở
(6, 3, 31, 12000000, 1), -- Túi Xách Tay Nữ
(7, 4, 4, 6500000, 2),  -- Áo Hoodie Nam Nỉ
(8, 4, 10, 5500000, 1), -- Quần Tây Nam Công Sở
(9, 5, 16, 2500000, 3), -- Áo Croptop Nữ
(10, 5, 22, 4500000, 1), -- Quần Culottes Nữ
(11, 6, 27, 8500000, 1), -- Đầm Maxi Dự Tiệc
(12, 6, 32, 8000000, 1), -- Túi Đeo Chéo
(13, 7, 5, 7500000, 1),  -- Áo Khoác Jean Nam
(14, 7, 11, 4500000, 2), -- Quần Kaki Nam
(15, 8, 17, 4500000, 1), -- Áo Sơ Mi Nữ
(16, 8, 23, 3500000, 2), -- Quần Short Nữ
(17, 9, 28, 4000000, 1), -- Váy Tennis
(18, 9, 33, 4000000, 1), -- Túi Tote
(19, 10, 6, 8500000, 1), -- Áo Blazer Nam Hàn Quốc
(20, 10, 12, 3500000, 2)  -- Quần Short Jean Nam
GO
SET IDENTITY_INSERT [dbo].[OrderDetail] OFF
GO

-- Contact data
SET IDENTITY_INSERT [dbo].[Contact] ON 
GO
INSERT [dbo].[Contact] ([ContactId], [Content], [Status], [EmailCC]) VALUES 
(1, N'Chào an huy', 1, N'ngocnguyen29696@gmail.com')
GO
SET IDENTITY_INSERT [dbo].[Contact] OFF
GO

INSERT [dbo].[News] ([NewsId], [Title], [Detail], [Photo], [DateUpdate]) VALUES
(1, N'Xu hướng thời trang xuân hè 2024', N'Khám phá những xu hướng thời trang mới nhất mùa xuân hè 2024...', N'fashion-trend-2024.jpg', CAST(N'2024-01-01' AS Date)),
(2, N'Cách phối đồ cho nam giới công sở', N'Hướng dẫn cách phối đồ nam thanh lịch cho dân văn phòng...', N'office-style.jpg', CAST(N'2024-01-05' AS Date)),
(3, N'Bí quyết chọn váy đầm dự tiệc', N'Gợi ý cách chọn váy đầm phù hợp cho các buổi tiệc...', N'party-dress.jpg', CAST(N'2024-01-10' AS Date)),
(4, N'Chăm sóc quần áo len đúng cách', N'Hướng dẫn cách giặt và bảo quản đồ len để bền đẹp...', N'wool-care.jpg', CAST(N'2024-01-15' AS Date))
GO

