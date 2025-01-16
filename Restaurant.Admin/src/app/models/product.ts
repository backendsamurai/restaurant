export interface ICreateProductModel {
	name: string;
	description: string;
	price: number;
	categoryId: string;
}

export interface IUpdateProductModel {
	name?: string;
	description?: string;
	price?: number;
	categoryId?: string;
}

export interface IProductCategory {
	id: string;
	name: string;
}

export interface IProduct {
	name: string;
	description: string;
	imageUrl: string;
	oldPrice?: number;
	price: number;
	category: IProductCategory;
}
