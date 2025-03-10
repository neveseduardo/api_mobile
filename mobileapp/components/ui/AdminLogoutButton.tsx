import { useAdminAuth } from '@/hooks/useAdminAuth';
import Ionicons from '@expo/vector-icons/Ionicons';
import { Text } from 'react-native';
import Button from './Button';
import { router } from 'expo-router';

const AdminLogoutButton = () => {
	const { logout } = useAdminAuth();

	async function onPressLogout() {
		await logout();
		router.replace('/(auth)/adminlogin');
	}
	return (
		<Button color="primary" className="flex flex-row w-full gap-2" onPress={onPressLogout}>
			<Ionicons name="log-out-outline" className="text-white" size={16} />
			<Text className="text-white">Logout</Text>
		</Button>
	);
};

export default AdminLogoutButton;