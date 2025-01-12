import c from '@ui/components';
import { FC, ReactNode } from 'react';

export const MainLayout: FC<{ children: ReactNode }> = ({ children }) => (
	<>
		<c.Header />
		<main>{children}</main>
	</>
);
