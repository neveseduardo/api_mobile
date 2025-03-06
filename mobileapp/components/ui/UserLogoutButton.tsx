import Ionicons from '@expo/vector-icons/Ionicons';
import { Text } from 'react-native';
import Button from './Button';
import { router } from 'expo-router';
import { useUserAuth } from '@/contexts/UserAuthenticationContext';

const UserLogoutButton = () => {
	const { logout } = useUserAuth();

	async function onPressLogout() {
		await logout();
		router.replace('/(auth)/userlogin');
	}
	return (
		<Button color="primary" className="flex flex-row w-full gap-2" onPress={onPressLogout}>
			<Ionicons name="log-out-outline" className="text-white" size={16} />
			<Text className="text-white">Logout</Text>
		</Button>
	);
};

export default UserLogoutButton;