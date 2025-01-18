import c from '@ui/components';
import { FC, ReactNode } from 'react';

interface Props {
	children: ReactNode;
}

export const MainLayout: FC<Props> = ({ children }) => (
	<>
		<c.Header />
		<main>{children}</main>
	</>
);
