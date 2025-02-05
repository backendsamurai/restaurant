import { IAuthenticationEmployeeResponse } from '_api/models';
import { Status } from '_types';

export interface IAuthState {
	user: IAuthenticationEmployeeResponse | null;
	status: Status;
	error?: string;
	authenticated?: boolean;
}
