import { ThemedView } from '@/components/ui/ThemedView';
import UserProfileHeader from '@/components/modules/user/UserProfileHeader';
import { useUserAuth } from '@/contexts/UserAuthenticationContext';
import Button from '../../../components/ui/Button';
import { Text } from 'react-native';
import { router } from 'expo-router';

const UserScreen = () => {
	const { userData } = useUserAuth();

	return (
		<ThemedView className="flex-1 w-full p-5">
			<ThemedView className="flex items-center justify-center w-full gap-4">
				<UserProfileHeader username={userData?.name} email={userData?.email} />
				<Button className="w-full" color="primary" onPress={() => router.push('/(userzone)/perfil/enderecos')}>
					<Text className="text-white">ENDEREÇOS</Text>
				</Button>
			</ThemedView>
		</ThemedView>
	);
};

export default UserScreen;