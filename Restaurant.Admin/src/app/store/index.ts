import { IS_DEVELOPMENT } from '@/config';
import { configureStore } from '@reduxjs/toolkit';
import authReducer from './reducers/auth';

export const store = configureStore({
	devTools: IS_DEVELOPMENT,
	reducer: {
		auth: authReducer,
	},
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
