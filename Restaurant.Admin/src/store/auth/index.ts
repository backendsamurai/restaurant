import { createSlice } from '@reduxjs/toolkit';
import { extraReducers, init, login, logout } from './reducers';
import { IAuthState } from './types';

const initialState: IAuthState = {
	status: 'idle',
	user: null,
};

const authSlice = createSlice({
	name: 'auth',
	initialState,
	reducers: {
		init,
		login,
		logout,
	},
	extraReducers: extraReducers,
});

export default authSlice.reducer;
export const {
	init: initAction,
	login: loginAction,
	logout: logoutAction,
} = authSlice.actions;
