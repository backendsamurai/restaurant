export interface IUser {
	id: string;
	userId: string;
	userName: string;
	userEmail: string;
	employeeRole?: string;
	isVerified: boolean;
	accessToken: string;
}
