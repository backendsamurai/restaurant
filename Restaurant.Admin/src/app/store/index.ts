import { configureStore } from '@reduxjs/toolkit';
import authReducer from './reducers/auth';

export const store = configureStore({
	devTools: true,
	reducer: {
		auth: authReducer,
	},
});

export type RootState = ReturnType<typeof store.getState>;
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch;
