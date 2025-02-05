import { ActionReducerMapBuilder } from '@reduxjs/toolkit';
import { IAuthenticationEmployeeResponse } from '_api/models';
import { WritableDraft } from 'immer';
import { authenticate } from './api';
import { IAuthState } from './types';

export const login = (
	state: WritableDraft<IAuthState>,
	action: { payload: IAuthenticationEmployeeResponse; type: string }
) => {
	localStorage.removeItem('user');
	localStorage.setItem('user', JSON.stringify(action.payload));
	state.user = action.payload;
	state.authenticated = true;
};

export const logout = (state: WritableDraft<IAuthState>) => {
	localStorage.removeItem('user');
	state.user = null;
	state.authenticated = false;
};

export const init = (state: WritableDraft<IAuthState>) => {
	const userJson = localStorage.getItem('user');
	if (userJson) {
		state.user = JSON.parse(userJson);
		state.authenticated = true;
	} else {
		state.authenticated = false;
	}
};

export const extraReducers = (builder: ActionReducerMapBuilder<IAuthState>) => {
	builder.addCase(authenticate.fulfilled, (state, action) => {
		if (action.payload) {
			state.status = 'fulfilled';
			login(state, { payload: action.payload, type: action.type });
		}
	});

	builder.addCase(authenticate.pending, (state, _) => {
		state.status = 'pending';
	});

	builder.addCase(authenticate.rejected, (state, { payload }) => {
		if (payload) {
			state.status = 'rejected';
			state.error = payload;
		}
	});
};
