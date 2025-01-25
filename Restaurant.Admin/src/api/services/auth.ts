import { IApiResponse, IAuthenticationEmployeeResponse } from '_api/models';
import { httpClient } from '_config';
import { AxiosResponse } from 'axios';

export interface IAuthenticateEmployeeParams {
	email: string;
	password: string;
}

export interface IAuthenticateEmployeeReturn
	extends IApiResponse<IAuthenticationEmployeeResponse> {}

export const authenticateEmployee = async (
	params: IAuthenticateEmployeeParams
): Promise<AxiosResponse<IAuthenticateEmployeeReturn>> =>
	await httpClient.post<IAuthenticateEmployeeReturn>(
		'/employees/authentication',
		params
	);
