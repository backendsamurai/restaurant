export interface IApiResponse<T> {
	data?: T;
	error?: IDetailedError;
	status: number;
}

export interface IDetailedError {
	type: string;
	code: string;
	message: string;
	detail: string;
}
