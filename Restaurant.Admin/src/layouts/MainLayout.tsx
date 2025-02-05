import { Container } from '@chakra-ui/react';
import { Header } from '_components/header';
import { FC, PropsWithChildren } from 'react';

export const MainLayout: FC<PropsWithChildren> = ({ children }) => (
	<>
		<Header />
		<Container>{children}</Container>
	</>
);
