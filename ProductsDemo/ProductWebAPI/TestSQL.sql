Select * from Product
Select * from ProductAttribute
Select * from ProductAttributeLookup
Select * from ProductCategory

/*

DELETE FROM ProductAttribute WHERE ProductId IN (5, 10002)
DELETE FROM Product WHERE ProductId IN (5, 10002)

ALTER TABLE [dbo].[ProductAttribute]
ADD CONSTRAINT [PK_ProductAttribute] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[AttributeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
ON [PRIMARY]

INSERT [dbo].[Product] ([ProdCatId], [ProdName], [ProdDescription]) VALUES (1, N'Swift', N'Maruti Suzuki Swift')
INSERT [dbo].[ProductAttribute] ([ProductId], [AttributeId], [AttributeValue]) VALUES (1, 1, N'Red')
INSERT [dbo].[ProductAttribute] ([ProductId], [AttributeId], [AttributeValue]) VALUES (1, 2, N'Petrol')
INSERT [dbo].[ProductAttribute] ([ProductId], [AttributeId], [AttributeValue]) VALUES (1, 3, N'VXI')
*/