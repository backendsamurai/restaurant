export interface ICreateEmployeeModel {
	name: string;
	email: string;
	password: string;
	role: string;
}

export interface IUpdateEmployeeModel {
	name?: string;
	email?: string;
	password?: string;
	role?: string;
}

export interface IEmployee {
	employeeId: string;
	userId: string;
	userName: string;
	userEmail: string;
	employeeRole: string;
	isVerified: boolean;
}

export interface IEmployeeRole {
	id: string;
	name: string;
}
