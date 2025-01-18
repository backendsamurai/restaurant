import { IUser } from '@/app/models/user';
import { createSlice } from '@reduxjs/toolkit';

export interface IAuthState {
	user?: IUser;
}

const initialState: IAuthState = {};

export const authSlice = createSlice({
	name: 'auth',
	initialState,
	reducers: {
		login: (state, action) => {
			localStorage.removeItem('user');
			localStorage.setItem('user', JSON.stringify(action.payload));
			state.user = action.payload;
		},
		setUser: (state, action) => {
			state.user = action.payload;
		},
	},
});

export default authSlice.reducer;
export const { login, setUser } = authSlice.actions;
