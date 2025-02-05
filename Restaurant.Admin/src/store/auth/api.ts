import { createAsyncThunk } from '@reduxjs/toolkit';
import { IAuthenticationEmployeeResponse } from '_api/models';
import {
	authenticateEmployee,
	IAuthenticateEmployeeParams,
} from '_api/services';
import axios from 'axios';

export const authenticate = createAsyncThunk<
	IAuthenticationEmployeeResponse | null,
	IAuthenticateEmployeeParams,
	{ rejectValue: string }
>('authentication', async (params, thunkApi) => {
	try {
		const response = await authenticateEmployee(params);
		if (response.status == 200) {
			return response.data?.data || null;
		}

		return thunkApi.rejectWithValue('Server unavailable!');
	} catch (e) {
		if (axios.isAxiosError(e) && e.response?.data?.error) {
			return thunkApi.rejectWithValue(e.response?.data?.error?.message);
		}

		return thunkApi.rejectWithValue('Unexpected error!');
	}
});
