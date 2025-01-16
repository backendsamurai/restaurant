import { IPayment } from './payment';

export interface ICreateOrderModel {
	customerId: string;
	waiterId: string;
	deskId: string;
	orderItems: Record<string, number>;
}

export enum OrderStatus {
	AwaitPayment,
	Pending,
	Closed,
}

export interface IOrderLineItem {
	productName: string;
	productPrice: number;
	count: number;
}

export interface IOrder {
	orderId: string;
	customerId: string;
	waiterId: string;
	deskId: string;
	status: OrderStatus;
	items: IOrderLineItem[];
	payment: IPayment;
	createdAt: string;
	updatedAt: string;
}
