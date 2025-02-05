export interface IAuthenticationEmployeeResponse {
	id: string;
	userId: string;
	userName: string;
	userEmail: string;
	employeeRole: string;
	accessToken: string;
	isVerified: boolean;
}
