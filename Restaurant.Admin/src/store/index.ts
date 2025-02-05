import { configureStore } from '@reduxjs/toolkit';
import { DEVELOPMENT } from '_config';
import authReducer from './auth';

export const store = configureStore({
	devTools: DEVELOPMENT,
	reducer: {
		auth: authReducer,
	},
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
