import { Heading, Table } from '@chakra-ui/react';
import { httpClient } from '_config';
import { MainLayout } from '_layouts';
import { FC, useEffect, useState } from 'react';

interface IDesk {
	id: string;
	name: string;
}

export const Desks: FC = () => {
	const [desks, setDesks] = useState<IDesk[]>([]);
	const [loading, setLoading] = useState<boolean>();

	useEffect(() => {
		const fetchDesks = async () => {
			setLoading(true);
			try {
				const response = await httpClient.get('/desks');

				await new Promise((r) => {
					setTimeout(() => {
						r(10);
					}, 2000);
				});
				if (response.status === 200) {
					setDesks(response.data?.data as IDesk[]);
				}
				setLoading(false);
			} catch (e) {
				setLoading(false);
			}
		};

		fetchDesks();
	}, []);

	return (
		<MainLayout>
			<Heading my={'2'}>Desks</Heading>
			{loading ? (
				<p>Loading....</p>
			) : (
				<Table.Root size='sm' variant='outline' striped>
					<Table.Header>
						<Table.Row>
							<Table.ColumnHeader>ID</Table.ColumnHeader>
							<Table.ColumnHeader>Name</Table.ColumnHeader>
						</Table.Row>
					</Table.Header>
					<Table.Body>
						{desks.map((desk) => (
							<Table.Row key={desk.id}>
								<Table.Cell>{desk.id}</Table.Cell>
								<Table.Cell>{desk.name}</Table.Cell>
							</Table.Row>
						))}
					</Table.Body>
				</Table.Root>
			)}
		</MainLayout>
	);
};
