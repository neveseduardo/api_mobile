import { View } from 'react-native';
import { useAdminAuth } from '@/contexts/AdminAuthenticationContext';
import { ThemedText } from './ThemedText';

const AdminDrawerContent = () => {

	const { userData } = useAdminAuth();

	return (
		<View className="flex flex-row items-center gap-4 p-5 rounded-lg bg-slate-100 dark:bg-slate-800">
			<View>
				<View className="w-[50px] h-[50px] bg-gray-900 rounded-full flex justify-center items-center">
					<ThemedText className="text-2xl font-bold text-white">{userData?.name.charAt(0)}</ThemedText>
				</View>
			</View>
			<View>
				<ThemedText className="font-semibold uppercase">{userData?.name}</ThemedText>
				<ThemedText className="">Bem vindo!</ThemedText>
			</View>
		</View>
	);
};

export default AdminDrawerContent;